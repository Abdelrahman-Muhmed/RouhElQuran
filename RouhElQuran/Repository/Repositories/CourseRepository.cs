using Core.IRepo;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repos
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        private readonly RouhElQuranContext dbContext;

        public CourseRepository(RouhElQuranContext _dbContext) : base(_dbContext)
           => dbContext = _dbContext;

		public async Task<Course?> GetCourseWithPlansByIDAsync(int id)
        {
            var GetData = await dbContext.Courses
                .Include(e => e.CoursePlans)
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();

            return GetData;
        }


    }
}