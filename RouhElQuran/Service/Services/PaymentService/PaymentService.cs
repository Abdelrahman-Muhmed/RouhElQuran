using Core.IRepo;
using Core.IServices.PaymentService;
using Core.Models;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;
using Stripe.Climate;
using System.Numerics;

namespace RouhElQuran.PaymentServices
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration configuration;
        private readonly IGenericrepo<CoursePlan> GenericRepo;
        private readonly IGenericrepo<UserPayments> UserPaymentRepo;

        public PaymentService(IConfiguration _configuration, IGenericrepo<CoursePlan> _CoursePlanRepo, IGenericrepo<UserPayments> _UserPaymentRepo)
        {
            configuration = _configuration;
            GenericRepo = _CoursePlanRepo;
            UserPaymentRepo = _UserPaymentRepo;
        }

        public async Task<Session?> CreateOrUpdatePaymentIntent(int CoursePlanId, string BuyerEmail)
        {
            StripeConfiguration.ApiKey = configuration["StripeSetting:Secretkey"];
            var CoursePlan = await GenericRepo.GetByIdAsync(CoursePlanId);
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)CoursePlan.price * 100,
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = CoursePlan.planName,
                        },
                    },
                    Quantity = 1,
                },
            },
                Mode = "payment",
                SuccessUrl = "http://localhost:4200/home",
                CancelUrl = "https://yourdomain.com/cancel",
            };

            var service = new SessionService();
            Session session = service.Create(options);

            var CreatePaymentForUser = new UserPayments(CoursePlan.courseId, CoursePlanId, BuyerEmail, (long)CoursePlan.price);

            await UserPaymentRepo.AddAsync(CreatePaymentForUser);
            return session;
        }

        public async Task<UserPayments> UpdatePaymentIntentToSuccededOrFailed(string PaymentIntentID, DateTime TimeCreated, bool IsSucceded)
        {
            var PaymentPlan = await UserPaymentRepo.GetLastElementByID(e => e.Id);
            if (PaymentPlan != null)
            {
                PaymentPlan.PaymentDate = TimeCreated;
                PaymentPlan.PaymentIntentID = PaymentIntentID;
                PaymentPlan.Status = IsSucceded ? PaymentStatus.PaymentReceived.ToString() : PaymentStatus.PaymentFailed.ToString();
                await UserPaymentRepo.UpdateAsync(PaymentPlan);
            }

            return PaymentPlan;
        }
    }
}