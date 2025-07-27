namespace Service.Dto_s
{
    public class StudentDto
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public int? CountHours { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PersonalImage { get; set; }
        public string? PhoneNumber { get; set; }
    }
}