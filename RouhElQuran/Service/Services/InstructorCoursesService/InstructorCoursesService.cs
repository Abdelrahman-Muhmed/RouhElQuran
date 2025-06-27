using AutoMapper;
using Core.Dto_s;
using Core.IRepo;
using Core.IServices.InstructorCoursesService;
using Core.Models;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

		public async Task<IEnumerable<Ins_Course>> CreateInstructorCourseAsync(InstructorCoursesDto instructorCoursesDto)
		{
			var instructorCourses = _mapper.Map<List<Ins_Course>>(instructorCoursesDto);

            await _instructorCoursesReository.createInstructorCours(instructorCourses);
			return instructorCourses;
		}

		public async Task<IEnumerable<InstructorCoursesDto>> GetInstructorCoursesAsync()
		{
		  var data = await _instructorCoursesReository.GetCourseWithInstructorGrouped();
		  var result = _mapper.Map<IEnumerable<InstructorCoursesDto>>(data);
		   return result;
		}

	
	}
}
