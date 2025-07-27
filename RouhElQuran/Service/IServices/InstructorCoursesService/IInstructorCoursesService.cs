
using Core.HelperModel.PaginationModel;
using Core.Models;
using Service.Dto_s;

namespace Core.IServices.InstructorCoursesService
{
	public interface IInstructorCoursesService
	{
		public IEnumerable<InstructorCoursesDto> GetInstructorCoursesAsync();
        PaginationRequest<InstructorCoursesDto> GetInstructorCoursesAsync(string sortBy, bool isDesc, int page, int pageSize);
        //PaginationRequest<IGrouping<int, Ins_Course>> GetInstructorCoursesAsync(string sortBy, bool isDesc, int page, int pageSize);


        public Task<IEnumerable<Ins_Course>> CreateInstructorCourseAsync(InstructorCoursesDto instructorCoursesDto);
        public Task<IEnumerable<Ins_Course>> UpdateInstructorCourseAsync(InstructorCoursesDto instructorCoursesDto);

        public Task<IEnumerable<InstructorCoursesDto>> GetInstructorCourseByInstructorId(int? id);
	
	}
}
