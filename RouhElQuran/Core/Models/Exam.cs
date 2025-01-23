using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class Exam
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public decimal? ExamResult { get; set; }

    public int? StdId { get; set; }

    public int? CourseId { get; set; }

    public virtual Course? Course { get; set; }

    public virtual Student? Std { get; set; }
}
