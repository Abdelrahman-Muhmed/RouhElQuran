using Core.IRepo;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace Talabat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IPaymentService _paymentService;
        private readonly IGenericrepo<UserPayments> GenericRepo;
        private const string WHSecret = "whsec_34a920ca553968b159084031499309aea5bb8afb1e9ce4389f6f77aba63f348d";

        public PaymentsController(IPaymentService paymentService, IGenericrepo<UserPayments> _GenericRepo, IConfiguration _configuration)
        {
            _paymentService = paymentService;
            GenericRepo = _GenericRepo;
            configuration = _configuration;
        }

        [HttpPost("CreatePayment")]
        [Authorize]
        public async Task<IActionResult> CreatePaymentPlan(int PlanId)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            //var BuyerEmail = "m.salah532@gmail.com";
            var PaymentSession = await _paymentService.CreateOrUpdatePaymentIntent(PlanId, BuyerEmail);
            if (PaymentSession != null)
                return Ok(new { sessionId = PaymentSession.Id });
            else
                return BadRequest();
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], WHSecret, throwOnApiVersionMismatch: false);

            // Handle the event
            var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;

            UserPayments userPayments = new UserPayments();

            switch (stripeEvent.Type)
            {
                case Events.PaymentIntentSucceeded:
                    userPayments = await _paymentService.UpdatePaymentIntentToSuccededOrFailed(paymentIntent.Id, paymentIntent.Created, true);
                    break;

                case Events.PaymentIntentPaymentFailed:
                    userPayments = await _paymentService.UpdatePaymentIntentToSuccededOrFailed(paymentIntent.Id, paymentIntent.Created, false);
                    break;
            }

            return Ok(userPayments);
        }
    }
}