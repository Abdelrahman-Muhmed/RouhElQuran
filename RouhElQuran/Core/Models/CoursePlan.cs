using Core.Enums;
using Repository.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class CoursePlan
    {
        public int ID { get; set; }
        public CoursePlansEnum Plan { get; set; }
        public decimal Price { get; set; }
        public int SessionCount { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }

        [NotMapped]
        public string? PaymentIntentId { get; set; }

        [NotMapped]
        public string? ClientSecret { get; set; }

        public virtual Course? Courses { get; set; }
    }
}