using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepo
{
    public interface ICourseRepository : IGenericrepo<Course>
    {
        public Task<Course> GetCourseWithPlansByIDAsync(int? id);

	}
}