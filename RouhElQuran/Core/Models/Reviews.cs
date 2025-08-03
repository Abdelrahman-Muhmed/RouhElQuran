using Repository.Models;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Reviews : BaseEntity
    {

        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string? Comment { get; set; }

        public int? CourseId { get; set; }
        public Course? Course { get; set; }

        public int? InstructorId { get; set; }
        public Instructor? Instructor { get; set; }

        public required int UserId { get; set; }
        public AppUser User { get; set; }
    }
}
