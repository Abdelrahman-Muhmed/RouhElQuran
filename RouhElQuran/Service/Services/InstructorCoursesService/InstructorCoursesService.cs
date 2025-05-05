using AutoMapper;
using Core.Dto_s;
using Core.IRepo;
using Core.IServices.InstructorCoursesService;
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

		public async Task<IEnumerable<InstructorCoursesDto>> GetInstructorCoursesAsync()
		{
		  var data = await _instructorCoursesReository.GetCourseWithInstructor();
		  var result = _mapper.Map<IEnumerable<InstructorCoursesDto>>(data);
		   return result;
		}
	}
}
