using Core.Models;

namespace RouhElQuran.Dto_s
{
    public class CourseDto
    {
        public int? Id { get; set; }

        public TimeOnly? SessionTime { get; set; }

        public string? Specialty { get; set; }

        public string? CrsName { get; set; }

        public int? CoursesTime { get; set; }

        public List<CoursePlanDto>? Course_Plan { get; set; } = new List<CoursePlanDto>();
    }
}