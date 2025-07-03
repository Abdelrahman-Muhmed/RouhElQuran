using Core.Dto_s;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices.InstructorCoursesService
{
	public interface IInstructorCoursesService
	{
		public IEnumerable<InstructorCoursesDto> GetInstructorCoursesAsync();
		IEnumerable<InstructorCoursesDto> GetInstructorCoursesAsync(string sortBy, bool sortDirection);

        public Task<IEnumerable<Ins_Course>> CreateInstructorCourseAsync(InstructorCoursesDto instructorCoursesDto);
        public Task<IEnumerable<Ins_Course>> UpdateInstructorCourseAsync(InstructorCoursesDto instructorCoursesDto);

        public Task<IEnumerable<InstructorCoursesDto>> GetInstructorCourseByInstructorId(int? id);
	
	}
}
