namespace RouhElQuran.Dto_s
{
    public class AttendenceDto
    {
        public int Id { get; set; }
        public string? Duration { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public DateOnly? Date { get; set; }
        public int? InsId { get; set; }
        public int? StdId { get; set; }
    }
}
