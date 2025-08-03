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

        public async Task<string?> CreateStripeCheckoutSessionAsync(int CoursePlanId, string BuyerEmail)
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
                            Name = CoursePlan.PlanNumber.ToString(),
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
                SessionId = session.Id
            };


            await _UnitOfWork.UserPaymentsRepository.AddAsync(CreatePaymentForUser);
            await _UnitOfWork.SaveChangesAsync();
            return session.Url;
        }

        public async Task UpdatePaymentIntentToSuccededOrFailed(string sessionID, bool IsSucceded)
        {
            var CurrentPayment = await _UnitOfWork.UserPaymentsRepository.GetFirstOrDefaultAsync(r => r.SessionId == sessionID);

            if (CurrentPayment != null)
            {
                CurrentPayment.PaymentDate = DateTime.Now;
                CurrentPayment.Status = IsSucceded ? PaymentStatus.PaymentReceived : PaymentStatus.PaymentFailed;
                _UnitOfWork.UserPaymentsRepository.Update(CurrentPayment);
                await _UnitOfWork.SaveChangesAsync();
            }

            return;
        }

        //TODO: Add Logs For Errors
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

                switch (stripeEvent.Type)
                {
                    case Events.CheckoutSessionCompleted:
                        await UpdatePaymentIntentToSuccededOrFailed(session.Id, true);
                        break;

                    case Events.CheckoutSessionExpired:
                        await UpdatePaymentIntentToSuccededOrFailed(session.Id, false);
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