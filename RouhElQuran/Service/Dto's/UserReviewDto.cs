using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto_s
{
    public class UserReviewDto
    {
        public int? CourseID { get; set; }
        public int? InstructorID { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string? Comment { get; set; }

        public int? CoursereviewCount { get; set; }
        public int? InstructorviewCount { get; set; }
        public string? reviewDate { get; set; }
        //public double? avergRate { get; set; }
        public string? UserName { get; set; }

    }
}
