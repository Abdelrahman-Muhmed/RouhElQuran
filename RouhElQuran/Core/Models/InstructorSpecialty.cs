using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
	public partial class InstructorSpecialty
	{
		public int Id { get; set; }
		public string? SpecialtyName { get; set; }
		public int? InstructorId { get; set; }

		public virtual Instructor? Instructor { get; set; }
	}
}
