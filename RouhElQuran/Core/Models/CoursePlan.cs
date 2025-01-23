using Repository.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class CoursePlan
    {
        public int id { get; set; }
        public string planName { get; set; }
        public decimal price { get; set; }
        public int sessionCount { get; set; }

        [ForeignKey("Course")]
        public int courseId { get; set; }

        [NotMapped]
        public string? PaymentIntentId { get; set; }

        [NotMapped]
        public string? ClientSecret { get; set; }

        public virtual Course? Courses { get; set; }
    }
}