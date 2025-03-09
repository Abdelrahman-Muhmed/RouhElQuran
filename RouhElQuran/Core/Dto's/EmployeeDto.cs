namespace RouhElQuran.Dto_s
{
    public class EmployeeDto
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public decimal? Salary { get; set; }
        public DateOnly? HireDate { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PersonalImage { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
