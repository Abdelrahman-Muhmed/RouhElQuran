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
    public class FreeClassRepository : Genericrepo<freeClass>, IFreeClassRepository
    {
        private readonly RouhElQuranContext dbContext;

        public FreeClassRepository(RouhElQuranContext _dbContext) : base(_dbContext)
           => dbContext = _dbContext;

        public async Task<bool> CheckUserAndCourse(int UserId, int CourseID)
        {
            var result = await dbContext.FreeClasses
                .Where(e => e.courseId == CourseID && e.AppUserId == UserId).Select(e => e.check).FirstOrDefaultAsync();
            return result;
        }

        public async Task<Course> GetCourseByID(int id)
          => await dbContext.Courses.FirstOrDefaultAsync(c => c.Id == id);
    }
}