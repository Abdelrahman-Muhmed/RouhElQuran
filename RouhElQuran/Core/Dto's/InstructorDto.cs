using Core.HelperModel.FileModel;

namespace Core.Dto_s
{
    public class InstructorDto
    {
        //public int? Id { get; set; }
        public int InstructorId { get; set; }

        public string? InstructorFirstName { get; set; }
        public string? InstructorLastName { get; set; }

        public List<string>? CoursesName { get; set; }
        public List<int> CourseIds { get; set; }
        public decimal? Salary { get; set; }
        public string? Certificate { get; set; }
        public string? Description { get; set; }
        public string? InstructorEmail { get; set; }
        public DateOnly WorkExperienceFrom { get; set; }
        public DateOnly WorkExperienceTo { get; set; }
        public string YearsOfExperience { get; set; }
        public TimeOnly? TimeFrom { get; set; }
        public TimeOnly? TimeTo { get; set; }
        public string? DaysWork { get; set; }
        public int InsUser_Id { get; set; }
        public bool IsActive { get; set; }
		public List<string?> FileName { get; set; }

        public FileUpload? FileUpload { get; set; }
        public List<CourseDto> courseDtos { get; set; }


    }
}