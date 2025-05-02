using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Dto_s
{
    public class CoursePlanDto
    {
        public int id { get; set; }
        public string planName { get; set; }
        public decimal price { get; set; }
        public int sessionCount { get; set; }
        public int courseId { get; set; }
    }
}