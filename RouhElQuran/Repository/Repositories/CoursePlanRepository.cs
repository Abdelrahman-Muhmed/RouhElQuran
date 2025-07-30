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
    public class CoursePlanRepository : GenericRepository<CoursePlan>, ICoursePlanRepository
    {
        private readonly RouhElQuranContext _DbContext;

        public CoursePlanRepository(RouhElQuranContext dbContext) : base(dbContext)
        {
            _DbContext = dbContext;
        }


    }
}