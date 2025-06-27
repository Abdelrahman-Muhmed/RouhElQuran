using Core.Dto_s;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepo
{
	public interface IInstructorCoursesReository : IGenericrepo<Ins_Course>
	{
        public Task<IEnumerable<IGrouping<int, Ins_Course>>> GetCourseWithInstructorGrouped();
		public Task<IEnumerable<Ins_Course>> CreateInstructorCourses(InstructorCoursesDto instructorCoursesDtodto);
		public Task<IEnumerable<Ins_Course>> UpdateInstructorCourse(InstructorCoursesDto instructorCoursesDto);
		public Task<IEnumerable<IGrouping<int, Ins_Course>>> GetCourseInstructorByInstructorIdGrouped(int? id);
	}

}
