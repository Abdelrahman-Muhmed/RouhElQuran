using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class Attendence
{
    public int Id { get; set; }
    public string? Duration { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public DateOnly? Date { get; set; }
    public int? InsId { get; set; }
    public int? StdId { get; set; }
    public virtual Instructor? Ins { get; set; }
    public virtual Student? Std { get; set; }
}