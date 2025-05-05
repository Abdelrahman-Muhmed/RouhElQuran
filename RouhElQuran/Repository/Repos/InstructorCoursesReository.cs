using Core.IRepo;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repos
{
	public class InstructorCoursesReository : Genericrepo<Ins_Course>, IInstructorCoursesReository
	{
		private readonly RouhElQuranContext _dbcontext;
        public InstructorCoursesReository(RouhElQuranContext dbcontext) : base (dbcontext)
		 => _dbcontext = dbcontext;


		public async Task<IEnumerable<Ins_Course>> GetCourseWithInstructor()
		{
			var result = await _dbcontext.Ins_Crs.Include(e => e.Instructor).Include(e => e.Course).ToListAsync();
			return result;
		}
	}

}
