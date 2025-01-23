using Repository.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class freeClass
    {
        public int id { get; set; }
        [ForeignKey("AppUser")]
        public int AppUserId { get; set; }
        [ForeignKey("Course")]
        public int courseId { get; set; }
        public bool check { get; set; } = false;
        public virtual AppUser? AppUser { get; set; } 
        public virtual Course? Course { get; set; } 
        
    }
}
