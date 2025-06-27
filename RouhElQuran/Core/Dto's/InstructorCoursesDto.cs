using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto_s
{
	public class InstructorCoursesDto
	{
		public InstructorDto instructorDtos { get; set; } = new InstructorDto();

		public List<CourseDto> courseDtos { get; set; } = new List<CourseDto>();

		public int? insId { get; set; }
		public List<int?> crsIds { get; set; }
	}
}
