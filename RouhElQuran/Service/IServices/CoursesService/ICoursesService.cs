using Repository.Models;
using Microsoft.AspNetCore.Http;
using Service.Dto_s;

namespace RouhElQuran.IServices.CoursesService
{
	public interface ICoursesService
	{
		public Task<IEnumerable<CourseDto>> GetAllCourse();
		public Task<CourseDto> GetCourseById(int? id);
		public Task<Course> CreateCource(CourseDto courseDto, HttpRequest request);
		public Task<Course> updateCourse(CourseDto courseDto);

		public Task<Course> DeleteCourse(int id);

	}
}
