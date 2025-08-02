using Microsoft.AspNetCore.Http;
using Repository.Helper;
using Repository.Models;
using Service.Dto_s;

namespace RouhElQuran.IServices.CoursesService
{
    public interface ICoursesService
    {
        public Task<ApiResponse<List<CourseDto>>> GetAllCourse();
        public Task<ApiResponse<CourseDto>> GetCourseById(int? id);
        public Task<ApiResponse<bool>> CreateCourse(CourseDto courseDto, HttpRequest request);
        public Task<ApiResponse<bool>> UpdateCourse(CourseDto courseDto);
        Task<List<CoursePlanDto>> GetCoursePlansByCourseId(int CourseId);

        public Task<ApiResponse<bool>> DeleteCourse(int id);

    }
}
