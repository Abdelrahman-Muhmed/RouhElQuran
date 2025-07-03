using AutoMapper;
using Core.Dto_s;
using Core.IRepo;
using Core.IServices.InstructorCoursesService;
using Core.Models;
using Repository.Models;
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
        public IEnumerable<InstructorCoursesDto> GetInstructorCoursesAsync(string sortBy, bool IsDesc)
        {
            var data = _instructorCoursesReository.GetCourseWithInstructorGroupedSorted(sortBy , IsDesc);

            var result = _mapper.Map<IEnumerable<InstructorCoursesDto>>(data);

            return result;
        }

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
