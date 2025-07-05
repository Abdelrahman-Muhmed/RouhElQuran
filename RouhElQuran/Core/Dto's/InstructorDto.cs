using Core.HelperModel.FileModel;

namespace Core.Dto_s
{
    public class InstructorDto
    {
        public string InsName { get; set; }
        public int? Id { get; set; }
        public List<int?> CourseId { get; set; }
        public decimal? Salary { get; set; }
        public string? Certificate { get; set; }
        public string? Description { get; set; }
        public string? InsEmail { get; set; }
        public DateOnly WorkExperienceFrom { get; set; }
        public DateOnly WorkExperienceTo { get; set; }
        public TimeOnly? TimeFrom { get; set; }
        public TimeOnly? TimeTo { get; set; }
        public string? DaysWork { get; set; }
        public int? InsUser_Id { get; set; }
        public bool IsActive { get; set; }
        //public int? User_id { get; set; }
        public FileUpload? FileUpload { get; set; }

    }
}