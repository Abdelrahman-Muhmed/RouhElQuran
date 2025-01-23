using Core.Models;
using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class Student
{
    public int Id { get; set; }
    public int? CountHours { get; set; }
    public int StdUser_Id { get; set; }
    public virtual ICollection<Attendence>? Attendences { get; set; } = new List<Attendence>();
    public virtual ICollection<Booking>? Bookings { get; set; } = new List<Booking>();
    public virtual ICollection<Exam>? Exams { get; set; } = new List<Exam>();
    public virtual AppUser? User_id { get; set; }
    public virtual ICollection<Student_Course>? Std_Courses { get; set; } = new List<Student_Course>();
}