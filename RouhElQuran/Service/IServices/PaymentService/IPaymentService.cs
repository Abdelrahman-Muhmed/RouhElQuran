using Core.Models;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepo
{
    public interface IPaymentService
    {
        Task<string?> CreateStripeCheckoutSessionAsync(int CoursePlanId, string BuyerEmail);
        Task<bool> WebHookData(string json, string RequestHeader);
        Task UpdatePaymentIntentToSuccededOrFailed(string sessionID, bool IsSucceded);
    }
}