using Core.HelperModel;
using Core.HelperModel.FileModel;
using Core.Models;

namespace Service.Dto_s
{
    public class CourseDto
    {
        public int Id { get; set; }

        public TimeOnly? SessionTime { get; set; }

        public string? Specialty { get; set; }

        public string? Description { get; set; }

        public string? CourseName { get; set; }

        public int? CoursesTime { get; set; }
        public decimal? CoursePrice { get; set; }

        public List<string?> FileName { get; set; }

        
        public FileUpload? FileUpload { get; set; }
        public List<CoursePlanDto>? Course_Plan { get; set; } = new List<CoursePlanDto>();
        public List<UserReviewDto>? UserReview { get; set; } = new List<UserReviewDto>();
        public double? avergRate { get; set; }




    }
}