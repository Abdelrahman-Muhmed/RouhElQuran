using Core.Models;
using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class Instructor : BaseEntity
{
    //public int InstructorId { get; set; }
    public decimal? Salary { get; set; }
    public string? Certificate { get; set; }
    public TimeOnly? TimeFrom { get; set; }
    public TimeOnly? TimeTo { get; set; }

    public string? DaysWork { get; set; }
    public string? Description { get; set; }
    public DateOnly WorkExperienceFrom { get; set; }
    public DateOnly WorkExperienceTo { get; set; }
    public string?  YearsOfExperience { get; set; }
    public int InsUser_Id { get; set; }
    public ICollection<InstructorSpecialty> InstructorSpecialty { get; set; } = new List<InstructorSpecialty>();
    public ICollection<Attendence> Attendences { get; set; } = new List<Attendence>();
    public AppUser AppUser { get; set; }
    public ICollection<Ins_Course> Ins_Courses { get; set; } = new List<Ins_Course>();
    public ICollection<Reviews> Review { get; set; } = new List<Reviews>();
}