using System.ComponentModel.DataAnnotations.Schema;

namespace RouhElQuran.Dto_s
{
    public class FreeClassDto
    {
        public int AppUserId { get; set; }
        public int courseId { get; set; }
        public bool check { get; set; } = false;
    }
}