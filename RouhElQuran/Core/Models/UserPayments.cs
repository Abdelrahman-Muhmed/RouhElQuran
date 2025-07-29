

using Core.IServices.PaymentService;

namespace Core.Models
{
    public class UserPayments
    {
        public int Id { get; set; }
        public int CrsId { get; set; }
        public int PlanId { get; set; }

        public string UserEmail { get; set; }

        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        public DateTime? PaymentDate { get; set; } = DateTime.Now;

        public long PriceOfPlan { get; set; }

        public string? PaymentIntentID { get; set; }
    }
}