

using Core.IServices.PaymentService;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class UserPayments
    {
        public int Id { get; set; }
        public int CrsId { get; set; }
        public int PlanId { get; set; }

        [MaxLength(128)]
        public string UserEmail { get; set; }

        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        public DateTime? PaymentDate { get; set; } = DateTime.Now;

        public long PriceOfPlan { get; set; }

        [MaxLength(512)]
        public string SessionId { get; set; }
    }
}