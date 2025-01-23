using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class Booking
{
    public int Id { get; set; }

    public int? StdId { get; set; }

    public int? CountDaysInWeek { get; set; }

    public TimeOnly? TimeLearning { get; set; }

    public string? DaysLearning { get; set; }

    public int? CourseId { get; set; }

    public virtual Course? Course { get; set; }

    public virtual Student? Std { get; set; }
}
