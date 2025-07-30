using Core.IRepo;
using Core.IServices.PaymentService;
using Core.IUnitOfWork;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Service.Services;
using Stripe;
using Stripe.Checkout;
using Stripe.Climate;
using System.Numerics;

namespace RouhElQuran.PaymentServices
{
    public class PaymentService : ServiceBase, IPaymentService
    {
        private readonly IConfiguration _Configuration;

        public PaymentService(IUnitOfWork unitOfWork, IConfiguration _configuration) : base(unitOfWork)
        {
            _Configuration = _configuration;
        }

        public async Task<string?> PaymentProcessing(int CoursePlanId, string BuyerEmail)
        {
            StripeConfiguration.ApiKey = _Configuration["Stripe:Secretkey"];
            var CoursePlan = await _UnitOfWork.CoursePlanRepository.GetByIdAsync(CoursePlanId);
            if (CoursePlan == null)
                return null;

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)CoursePlan.Price * 100,
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = CoursePlan.Plan.ToString(),
                        },
                    },
                    Quantity = 1,
                },
            },
                Metadata = new Dictionary<string, string>
                {
                    { "CourseId", CoursePlan.CourseId.ToString() },
                    { "PlanId", CoursePlanId.ToString() },
                    { "UserEmail", BuyerEmail }
                },
                Mode = "payment",
                SuccessUrl = "http://localhost:4200/home",
                CancelUrl = "https://localhost:4200/cancel",
            };

            var service = new SessionService();
            Session session = service.Create(options);

            var CreatePaymentForUser = new UserPayments
            {
                CrsId = CoursePlan.CourseId,
                PlanId = CoursePlanId,
                UserEmail = BuyerEmail,
                PriceOfPlan = (long)CoursePlan.Price,
            };


            await _UnitOfWork.UserPaymentsRepository.AddAsync(CreatePaymentForUser);
            await _UnitOfWork.SaveChangesAsync();
            return session.Url;
        }

        public async Task<UserPayments?> UpdatePaymentIntentToSuccededOrFailed(string userEmail, DateTime TimeCreated, bool IsSucceded)
        {
            var PaymentPlan = await _UnitOfWork.UserPaymentsRepository.GetAllAsync().Where(e => e.UserEmail == userEmail).OrderByDescending(s => s.Id).FirstOrDefaultAsync();
            if (PaymentPlan != null)
            {
                PaymentPlan.PaymentDate = TimeCreated;
                PaymentPlan.Status = IsSucceded ? PaymentStatus.PaymentReceived : PaymentStatus.PaymentFailed;
                await _UnitOfWork.UserPaymentsRepository.UpdateAsync(PaymentPlan);
                await _UnitOfWork.SaveChangesAsync();
            }

            return PaymentPlan;
        }

        //TODO: Add Table Logs For Errors
        public async Task<bool> WebHookData(string json, string RequestHeader)
        {

            if (string.IsNullOrEmpty(RequestHeader))
            {
                return false;
            }
            Event stripeEvent;

            try
            {
                stripeEvent = EventUtility.ConstructEvent(json,
                    RequestHeader,
                    _Configuration["Stripe:WebhookSecret"],
                    throwOnApiVersionMismatch: false
                );

                if (stripeEvent.Data.Object is not Session session)
                {
                    //return BadRequest("Invalid session data.");
                    return false;

                }

                var userEmail = session.Metadata["UserEmail"];

                switch (stripeEvent.Type)
                {
                    case Events.CheckoutSessionCompleted:
                        await UpdatePaymentIntentToSuccededOrFailed(userEmail, DateTime.Now, true);
                        break;

                    case Events.CheckoutSessionExpired:
                        await UpdatePaymentIntentToSuccededOrFailed(userEmail, DateTime.Now, false);
                        break;
                }
                return true;
            }
            catch (StripeException)
            {
                //return BadRequest($"Webhook Error: {e.Message}");
                return false;
            }
        }

    }
}