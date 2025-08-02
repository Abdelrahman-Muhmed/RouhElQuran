using Microsoft.AspNetCore.Http;
using Repository.Helper;
using Repository.Models;
using Service.Dto_s;

namespace Core.IServices.InstructorService
{
    public interface IInstructorService
    {
        Task<ApiResponse<List<InstructorDto>>> GetAllInstructors();
        Task<ApiResponse<InstructorDto>> GetInstructorById(int id);
        Task<ApiResponse<int>> CreateInstructor(InstructorDto instructorDto, HttpRequest request);
        Task<ApiResponse<int>> UpdateInstructor(InstructorDto instructorDto);
        Task<ApiResponse<bool>> DeleteInstructor(int id);
    }
}
