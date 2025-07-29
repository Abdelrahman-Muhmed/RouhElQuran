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
        Task<string?> PaymentProcessing(int CoursePlanId, string BuyerEmail);
        Task<bool> WebHookData(string json, string RequestHeader);
        Task<UserPayments?> UpdatePaymentIntentToSuccededOrFailed(string userEmail, DateTime TimeCreated, bool IsSucceded);
    }
}