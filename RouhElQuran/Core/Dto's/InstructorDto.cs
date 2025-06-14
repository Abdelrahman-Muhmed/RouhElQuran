namespace Core.Dto_s
{
    public class InstructorDto
    {

        public string InsName { get; set; }
        public int? InstructorId { get; set; }

		public List<int?> CourseId { get; set; }

		public decimal? Salary { get; set; }

        public string? Certificate { get; set; }

        public string? TimeWork { get; set; }

     
		public string? DaysWork { get; set; }
        public int? InsUser_Id { get; set; }
    }
}