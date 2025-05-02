using Core.IServices.PaymentService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class UserPayments
    {
        public UserPayments()
        {
        }

        public UserPayments(int crsId, int planId, string userEmail, long price)
        {
            CrsId = crsId;
            PlanId = planId;
            UserEmail = userEmail;
            PriceOfPlan = price;
        }

        public int Id { get; set; }
        public int CrsId { get; set; }
        public int PlanId { get; set; }

        public string UserEmail { get; set; }

        public string Status { get; set; } = PaymentStatus.Pending.ToString();

        public DateTime? PaymentDate { get; set; }

        public long PriceOfPlan { get; set; }

        public string? PaymentIntentID { get; set; }
    }
}