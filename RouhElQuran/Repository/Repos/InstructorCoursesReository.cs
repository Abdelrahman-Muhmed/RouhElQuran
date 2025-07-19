using Azure;
using Core.Dto_s;
using Core.HelperModel.PaginationModel;
using Core.IRepo;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Helper.SortHelper;
using Repository.Models;
using Repository.Helper.PaginationHelper;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repos
{
	public class InstructorCoursesReository : Genericrepo<Ins_Course>, IInstructorCoursesReository
	{
		private readonly RouhElQuranContext _dbcontext;
		public InstructorCoursesReository(RouhElQuranContext dbcontext) : base(dbcontext)
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

   
        public async Task<IEnumerable<Ins_Course>> CreateInstructorCourses(InstructorCoursesDto instructorCoursesDto)
        {
            // Create the list of Ins_Course entities from the DTO
            var insCourses = instructorCoursesDto.crsIds
                .Where(courseId =>  instructorCoursesDto.insId.HasValue)
                .Select(courseId => new Ins_Course
                {
                    Ins_Id = instructorCoursesDto.insId.Value,
                    Course_Id = courseId
                })
                .ToList();

            if (insCourses.Any())
            {
                await _dbcontext.Ins_Crs.AddRangeAsync(insCourses);
                await _dbcontext.SaveChangesAsync();
            }

            return insCourses;
        }
        public async Task<IEnumerable<Ins_Course>> UpdateInstructorCourse(InstructorCoursesDto instructorCoursesDto)
        {
            if (instructorCoursesDto.insId == null)
                return Enumerable.Empty<Ins_Course>();

            // 1. Remove existing courses for this instructor
            var existing = _dbcontext.Ins_Crs.Where(x => x.Ins_Id == instructorCoursesDto.insId.Value);
             _dbcontext.Ins_Crs.RemoveRange(existing);

            // 2. Add new courses
            var insCourses = instructorCoursesDto.crsIds
                .Select(courseId => new Ins_Course
                {
                    Ins_Id = instructorCoursesDto.insId.Value,
                    Course_Id = courseId
                })
                .ToList();

            if (insCourses.Any())
            {
                _dbcontext.Ins_Crs.AddRange(insCourses);
            }

            _dbcontext.SaveChanges();
            return insCourses;

        }

     
    }

}
