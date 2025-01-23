using Core.Models;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepo
{
    public interface IFreeClassRepository : IGenericrepo<freeClass>
    {
        public Task<Course> GetCourseByID(int id);

        public Task<bool> CheckUserAndCourse(int UserId, int CourseID);
    }
}