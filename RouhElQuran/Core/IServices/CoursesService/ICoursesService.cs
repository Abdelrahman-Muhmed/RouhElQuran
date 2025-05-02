using Repository.Models;
using Core.Dto_s;
using Microsoft.AspNetCore.Http;

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
