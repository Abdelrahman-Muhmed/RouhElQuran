using Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Service.Dto_s
{
    public class CoursePlanDto
    {
        public int ID { get; set; }
        public CoursePlansEnum PlanNumber { get; set; }
        public string PlanName { get; set; }
        public decimal Price { get; set; }
        public int SessionCount { get; set; }
        public int CourseId { get; set; }
    }
}