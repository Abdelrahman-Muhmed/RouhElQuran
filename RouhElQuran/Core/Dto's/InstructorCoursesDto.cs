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

		public CourseDto courseDtos { get; set; } = new CourseDto();
	}
}
