using AutoMapper;
using Core.Dto_s;
using Core.HelperModel.PaginationModel;
using Core.IRepo;
using Core.IServices.InstructorCoursesService;
using Core.Models;
using Repository.Models;
using Repository.PaginationHelper;
using Service.Helper.SortHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.InstructorCoursesService
{
	public class InstructorCoursesService : IInstructorCoursesService
	{
		private readonly IInstructorCoursesReository _instructorCoursesReository;
		private readonly IMapper _mapper;
        public InstructorCoursesService(IInstructorCoursesReository instructorCoursesReository, IMapper mapper)
		{
		    _instructorCoursesReository = instructorCoursesReository;
			_mapper = mapper;
		}
        public IEnumerable<InstructorCoursesDto> GetInstructorCoursesAsync()
        {
            var data = _instructorCoursesReository.GetCourseWithInstructorGrouped();
            
            var result = _mapper.Map<IEnumerable<InstructorCoursesDto>>(data);
            return result;
        }
        public PaginationRequest<InstructorCoursesDto> GetInstructorCoursesAsync(string sortBy, bool IsDesc, int page, int pageSize)
        {
            var data = _instructorCoursesReository.GetCourseWithInstructorGroupedSorted(sortBy, IsDesc);

            var result = _mapper.Map<IEnumerable<InstructorCoursesDto>>(data);
            int totalCount = result.Count();

           var resultData =  PaginationHelper.CreatePaginatedResult<InstructorCoursesDto>(result, page, pageSize, totalCount , sortBy , IsDesc);

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
            var result = await _instructorCoursesReository.GetCourseInstructorByInstructorIdGrouped(id);
            var resultMap = _mapper.Map<IEnumerable<InstructorCoursesDto>>(result);
            return resultMap;
        }
        public async Task<IEnumerable<Ins_Course>> CreateInstructorCourseAsync(InstructorCoursesDto instructorCoursesDto)
		{
			//var instructorCourses = _mapper.Map<List<Ins_Course>>(instructorCoursesDto);

           var instructorCourses = await _instructorCoursesReository.CreateInstructorCourses(instructorCoursesDto);
			return instructorCourses;
		}

        public async Task<IEnumerable<Ins_Course>> UpdateInstructorCourseAsync(InstructorCoursesDto instructorCoursesDto)
        {
            var instructorCourses = await _instructorCoursesReository.UpdateInstructorCourse(instructorCoursesDto);
            return instructorCourses;
        }






    }
}
