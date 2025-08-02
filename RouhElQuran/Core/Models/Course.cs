using Core.Models;
using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class Course
{
    public int Id { get; set; }

    public TimeOnly? SessionTime { get; set; }

    public string? Specialty { get; set; }

	public string? Description { get; set; }

	public string? CourseName { get; set; }

    public int? CoursesTime { get; set; }
    public decimal? CoursePrice { get; set; }



    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public virtual ICollection<CoursePlan> CoursePlans { get; set; } = new List<CoursePlan>();
    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();
    public virtual ICollection<Student_Course> Std_Course { get; set; } = new List<Student_Course>();
    public virtual ICollection<Ins_Course> Ins_Courses { get; set; } = new List<Ins_Course>();
	public virtual ICollection<Files> files { get; set; } = new List<Files>();
	public virtual ICollection<Reviews> Review { get; set; } = new List<Reviews>();

}