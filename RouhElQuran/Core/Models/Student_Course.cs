using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Student_Course
    {
        public int Id { get; set; }
        public int Std_Id { get; set; }
        public int Course_Id { get; set; }
        public virtual Student students { get; set; }
        public virtual Course Course { get; set; }
    }
}