using Repository.Models;
using RouhElQuran.Dto_s;

namespace RouhElQuran.IServices.CoursesService
{
	public interface ICoursesService
	{
		public Task<IEnumerable<CourseDto>> GetAllCourse();
		public Task<CourseDto> GetCourseById(int id);
		public Task<Course> CreateCource(CourseDto courseDto);
		public Task<Course> updateCourse(CourseDto courseDto);

		public Task<Course> DeleteCourse(int id);

	}
}
