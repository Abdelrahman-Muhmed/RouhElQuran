using Core.Models;
using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class Instructor
{
    public int InstructorId { get; set; }

    public string? CurrentCourse { get; set; }

    public decimal? Salary { get; set; }

    public string? Certificate { get; set; }

    public string? TimeWork { get; set; }

    public string? DaysWork { get; set; }
    public int InsUser_Id { get; set; }

    public virtual ICollection<Attendence> Attendences { get; set; } = new List<Attendence>();
    public virtual AppUser User_id { get; set; }
    public virtual ICollection<Ins_Course> Ins_Courses { get; set; } = new List<Ins_Course>();
}