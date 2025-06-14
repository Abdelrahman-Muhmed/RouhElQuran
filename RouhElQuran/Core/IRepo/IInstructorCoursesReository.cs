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
        public Task<IEnumerable<Ins_Course>> GetCourseWithInstructor();
	
		public Task<IEnumerable<Ins_Course>> createInstructorCours(List<Ins_Course> Ins_Course);
	}

}
