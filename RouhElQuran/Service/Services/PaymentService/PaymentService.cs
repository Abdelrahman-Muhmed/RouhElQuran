using Core.IRepo;
using Core.IServices.PaymentService;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;
using Stripe.Climate;
using System.Numerics;

namespace RouhElQuran.PaymentServices
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _Configuration;
        private readonly IGenericrepo<CoursePlan> _GenericRepo;
        private readonly IGenericrepo<UserPayments> _UserPaymentRepo;

        public PaymentService(IConfiguration _configuration, IGenericrepo<CoursePlan> _CoursePlanRepo, IGenericrepo<UserPayments> _UserPaymentRepo)
        {
            _Configuration = _configuration;
            _GenericRepo = _CoursePlanRepo;
            this._UserPaymentRepo = _UserPaymentRepo;
        }

        public async Task<string?> PaymentProcessing(int CoursePlanId, string BuyerEmail)
        {
            StripeConfiguration.ApiKey = _Configuration["Stripe:Secretkey"];
            var CoursePlan = await _GenericRepo.GetByIdAsync(CoursePlanId);
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


            await _UserPaymentRepo.AddAsync(CreatePaymentForUser);
            return session.Url;
        }

        public async Task<UserPayments?> UpdatePaymentIntentToSuccededOrFailed(string userEmail, DateTime TimeCreated, bool IsSucceded)
        {
            var PaymentPlan = await _UserPaymentRepo.GetAllAsync().Where(e => e.UserEmail == userEmail).OrderByDescending(s => s.Id).FirstOrDefaultAsync();
            if (PaymentPlan != null)
            {
                PaymentPlan.PaymentDate = TimeCreated;
                PaymentPlan.Status = IsSucceded ? PaymentStatus.PaymentReceived : PaymentStatus.PaymentFailed;
                await _UserPaymentRepo.UpdateAsync(PaymentPlan);
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
            catch (StripeException e)
            {
                //return BadRequest($"Webhook Error: {e.Message}");
                return false;
            }
        }

    }
}