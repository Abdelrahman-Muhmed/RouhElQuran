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
        //Task<Session?> CreateOrUpdatePaymentIntent(int CoursePlanId, string BuyerEmail);

        //Task<UserPayments> UpdatePaymentIntentToSuccededOrFailed(string PaymentIntentID, DateTime TimeCreated, bool IsSucceded);
    }
}