using Core.IRepo;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Talabat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _PaymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _PaymentService = paymentService;
        }

        [HttpPost("CreatePayment")]
        [Authorize]
        public async Task<IActionResult> CreatePaymentPlan(int PlanId)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(BuyerEmail))
                return BadRequest("Please Login First");

            var paymentUrl = await _PaymentService.PaymentResult(PlanId, BuyerEmail);
            if (paymentUrl != null)
                return Ok(new { PaymentUrl = paymentUrl });
            else
                return BadRequest();
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var requestHeader = Request.Headers["Stripe-Signature"];

            var result = await _PaymentService.WebHookData(json, requestHeader!);

            if (result)
                return Ok();
            else
                return BadRequest("Webhook Error");
        }
    }
}