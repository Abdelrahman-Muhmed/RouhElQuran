using Core.Dto_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices.InstructorCoursesService
{
	public interface IInstructorCoursesService
	{
		public Task <IEnumerable<InstructorCoursesDto>> GetInstructorCoursesAsync();
	
	}
}
