using AutoMapper;
using Core.HelperModel.PaginationModel;
using Core.IRepo;
using Core.IServices.InstructorCoursesService;
using Core.Models;
using Repository.Helper.PaginationHelper;
using Repository.Models;
using Repository.Repos;
using Service.Dto_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.InstructorCoursesService
{
    public class InstructorCoursesService : IInstructorCoursesService
    {
        private readonly IInstructorCoursesRepository _InstructorCoursesReository;
        private readonly IMapper _mapper;
        public InstructorCoursesService(IInstructorCoursesRepository instructorCoursesReository, IMapper mapper)
        {
            _InstructorCoursesReository = instructorCoursesReository;
            _mapper = mapper;
        }
        public IEnumerable<InstructorCoursesDto> GetInstructorCoursesAsync()
        {
            var data = _InstructorCoursesReository.GetCourseWithInstructorGrouped();

            var result = _mapper.Map<IEnumerable<InstructorCoursesDto>>(data);
            return result;
        }
        public PaginationRequest<InstructorCoursesDto> GetInstructorCoursesAsync(string sortBy, bool IsDesc, int page, int pageSize)
        {
            var data = _InstructorCoursesReository.GetCourseWithInstructorGroupedSorted(sortBy, IsDesc);

            var result = _mapper.Map<IEnumerable<InstructorCoursesDto>>(data);
            int totalCount = result.Count();

            var resultData = PaginationHelper.CreatePaginatedResult<InstructorCoursesDto>(result, page, pageSize, totalCount, sortBy, IsDesc);

            return resultData;
        }

        //public PaginationRequest<IGrouping<int, Ins_Course>> GetInstructorCoursesAsync(string sortBy, bool IsDesc, int page, int pageSize)
        //{
        //    var data = _instructorCoursesReository.GetCourseWithInstructorGroupedSorted(sortBy, IsDesc, page, pageSize);

        //    //var result = _mapper.Map<IEnumerable<InstructorCoursesDto>>(data);

        //    return data;
        //}


        public async Task<IEnumerable<InstructorCoursesDto>> GetInstructorCourseByInstructorId(int? id)
        {
            var result = await _InstructorCoursesReository.GetCourseInstructorByInstructorIdGrouped(id);
            var resultMap = _mapper.Map<IEnumerable<InstructorCoursesDto>>(result);
            return resultMap;
        }
        // TODO: Implement the CreateInstructorCourseAsync and UpdateInstructorCourseAsync methods
        public async Task<IEnumerable<Ins_Course>> CreateInstructorCourseAsync(InstructorCoursesDto instructorCoursesDto)
        {
            var insCourses = instructorCoursesDto.crsIds
               .Where(courseId => instructorCoursesDto.insId.HasValue)
               .Select(courseId => new Ins_Course
               {
                   Ins_Id = instructorCoursesDto.insId.Value,
                   Course_Id = courseId
               })
               .ToList();

            var instructorCourses = await _InstructorCoursesReository.CreateInstructorCourses(insCourses);

            return instructorCourses;
        }

        public async Task<IEnumerable<Ins_Course>> UpdateInstructorCourseAsync(InstructorCoursesDto instructorCoursesDto)
        {
            if (instructorCoursesDto.insId == null)
                throw new ArgumentException("Instructor ID is required");

            if (instructorCoursesDto.crsIds == null || !instructorCoursesDto.crsIds.Any())
                throw new ArgumentException("At least one course must be assigned");

            var insCourses = instructorCoursesDto.crsIds
                .Select(courseId => new Ins_Course
                {
                    Ins_Id = instructorCoursesDto.insId.Value,
                    Course_Id = courseId
                }).ToList();

            var updatedCourses = await _InstructorCoursesReository.UpdateInstructorCourse(insCourses, instructorCoursesDto.insId.Value);
            return updatedCourses;
        }





    }
}
