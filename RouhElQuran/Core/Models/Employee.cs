using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class Employee
{
    public int Id { get; set; }
    public decimal? Salary { get; set; }
    public DateOnly? HireDate { get; set; }
    public int EmpUser_Id { get; set; }
    public virtual AppUser User_Id { get; set; }
}