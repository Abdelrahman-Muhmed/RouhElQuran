
using Core.IRepo;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Helper.SortHelper;
using Repository.Models;
using System.Linq.Dynamic.Core;
namespace Repository.Repos
{
	public class InstructorCoursesRepository : GenericRepository<Ins_Course>, IInstructorCoursesRepository
	{
		private readonly RouhElQuranContext _dbcontext;
		public InstructorCoursesRepository(RouhElQuranContext dbcontext) : base(dbcontext)
		 => _dbcontext = dbcontext;


        public  IQueryable<IGrouping<int, Ins_Course>> GetCourseWithInstructorGrouped()
        {
            return _dbcontext.Ins_Crs
              .Include(e => e.Instructor)
              .ThenInclude(i => i.AppUser)
              .Include(e => e.Course)
              .GroupBy(ic => ic.Ins_Id);

        }

        public IEnumerable<IGrouping<int, Ins_Course>> GetCourseWithInstructorGroupedSorted(string sortBy, bool isDesc)
        {
            var query = _dbcontext.Ins_Crs
            .Include(x => x.Instructor)
            .ThenInclude(i => i.AppUser)
            .ThenInclude(f => f.files)
            .Include(x => x.Course)
            .AsEnumerable()
            .GroupBy(x => x.Ins_Id);

            query = query.OrderGroupByProperty<int, Ins_Course>(sortBy, isDesc);

            return query;

        }

        public async Task<IEnumerable<IGrouping<int, Ins_Course>>> GetCourseInstructorByInstructorIdGrouped(int? id)
        {
            var result = await _dbcontext.Ins_Crs
                .Include(x => x.Instructor)
                .Include(x => x.Course)
                .Include(x => x.Instructor.AppUser)
                .Where(x => x.Ins_Id == id)
                .GroupBy(x => x.Ins_Id)
                .ToListAsync();

            return result;
        }

        // TODO: Uncomment and implement the methods below if needed
        public async Task<IEnumerable<Ins_Course>> CreateInstructorCourses(List<Ins_Course> InsCourses)
        {
            if (InsCourses.Any())
            {
                await _dbcontext.Ins_Crs.AddRangeAsync(InsCourses);
                await _dbcontext.SaveChangesAsync();
            }

            return InsCourses;
        }

        public async Task<IEnumerable<Ins_Course>> UpdateInstructorCourse(List<Ins_Course> insCourses, int instructorId)
        {
            var existing = _dbcontext.Ins_Crs.Where(x => x.Ins_Id == instructorId);
            _dbcontext.Ins_Crs.RemoveRange(existing);

            if (insCourses.Any())
                _dbcontext.Ins_Crs.AddRange(insCourses);

            await _dbcontext.SaveChangesAsync();
            return insCourses;
        }

    }

}
