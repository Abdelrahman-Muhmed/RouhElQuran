using AutoMapper;
using Core.IRepo;
using Core.IServices.InstructorService;
using Core.IUnitOfWork;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repository.Helper;
using Repository.Models;
using Service.Dto_s;
using Service.Helper.CalculatHelper;
using Service.Helper.FileUploadHelper;

namespace Service.Services.InstructorService
{
    public class InstructorService : ServiceBase, IInstructorService
    {

        private readonly IMapper _mapper;
        public InstructorService(IMapper mapper, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<InstructorDto>>> GetAllInstructors()
        {
            try
            {
                var instructorDto = await _UnitOfWork.InstructorRepository.SelectListAsync(null,
                    i => new InstructorDto
                    {
                        InstructorId = i.Id,
                        InsUser_Id = i.InsUser_Id,
                        InstructorFirstName = i.AppUser.FirstName,
                        InstructorLastName = i.AppUser.LastName,
                        Salary = i.Salary,
                        Certificate = i.Certificate,
                        TimeFrom = i.TimeFrom,
                        TimeTo = i.TimeTo,
                        DaysWork = i.DaysWork,
                        Description = i.Description,
                        WorkExperienceFrom = i.WorkExperienceFrom,
                        WorkExperienceTo = i.WorkExperienceTo,
                        YearsOfExperience = CalculatHelper.calculatYearsOfExperience(i.WorkExperienceTo, i.WorkExperienceFrom),
                        InstructorEmail = i.AppUser.Email,
                        CoursesName = i.Ins_Courses.Select(ic => ic.Course.CourseName).ToList(),
                        CourseIds = i.Ins_Courses.Select(ic => ic.Course_Id).ToList(),
                        FileName = i.AppUser.files.Select(f => f.UntrustedName).ToList(),
                    }, u => u.AppUser, ic => ic.Ins_Courses);

                return new ApiResponse<List<InstructorDto>>(instructorDto);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<InstructorDto>>($"Error occurred: {ex.Message}");
            }
        }


        public async Task<ApiResponse<InstructorDto>> GetInstructorById(int id)
        {
            try
            {
                var instructorDto = await _UnitOfWork.InstructorRepository.SelectFirstOrDefaultAsync(
                    c => c.Id == id,
                    i => new InstructorDto
                    {
                        InstructorId = i.Id,
                        InsUser_Id = i.InsUser_Id,
                        InstructorFirstName = i.AppUser.FirstName,
                        InstructorLastName = i.AppUser.LastName,
                        Salary = i.Salary,
                        Certificate = i.Certificate,
                        TimeFrom = i.TimeFrom,
                        TimeTo = i.TimeTo,
                        DaysWork = i.DaysWork,
                        Description = i.Description,
                        WorkExperienceFrom = i.WorkExperienceFrom,
                        WorkExperienceTo = i.WorkExperienceTo,
                        YearsOfExperience = CalculatHelper.calculatYearsOfExperience(i.WorkExperienceTo, i.WorkExperienceFrom),
                        InstructorEmail = i.AppUser.Email,
                        CoursesName = i.Ins_Courses.Select(ic => ic.Course.CourseName).ToList(),
                        CourseIds = i.Ins_Courses.Select(ic => ic.Course_Id).ToList(),
                        FileName = i.AppUser.files.Select(f => f.UntrustedName).ToList(),
                        courseDtos = i.Ins_Courses.Select(ic => new CourseDto
                        {
                            Id = ic.Course.Id,
                            CourseName = ic.Course.CourseName,
                            CoursePrice = ic.Course.CoursePrice,
                            CoursesTime = ic.Course.CoursesTime,
                            Specialty = ic.Course.Specialty,
                            SessionTime = ic.Course.SessionTime,
                            FileName = ic.Course.files.Select(f => f.UntrustedName).ToList()
                        }).ToList(),
                    }, u => u.AppUser, ic => ic.Ins_Courses);

                if (instructorDto is null)
                    return new ApiResponse<InstructorDto>("Instructor not found");

                return new ApiResponse<InstructorDto>(instructorDto);
            }
            catch (Exception ex)
            {
                return new ApiResponse<InstructorDto>($"Error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse<int>> CreateInstructor(InstructorDto instructorDto, HttpRequest request)
        {
            try
            {
                var instructor = new Instructor
                {
                    Salary = instructorDto.Salary,
                    Certificate = instructorDto.Certificate,
                    Description = instructorDto.Description,
                    TimeFrom = instructorDto.TimeFrom,
                    TimeTo = instructorDto.TimeTo,
                    DaysWork = instructorDto.DaysWork,
                    WorkExperienceFrom = instructorDto.WorkExperienceFrom,
                    WorkExperienceTo = instructorDto.WorkExperienceTo,
                    InsUser_Id = instructorDto.InsUser_Id,
                    YearsOfExperience = CalculatHelper.calculatYearsOfExperience(
                        instructorDto.WorkExperienceTo, instructorDto.WorkExperienceFrom),
                };

                await _UnitOfWork.InstructorRepository.AddAsync(instructor);
                await _UnitOfWork.SaveChangesAsync();

                await FileHelper.streamedOrBufferedProcess(request, instructorDto.FileUpload, _UnitOfWork.FilesRepository, userId: instructorDto.InsUser_Id);
                await _UnitOfWork.SaveChangesAsync();

                return new ApiResponse<int>(instructor.Id, "Instructor created successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<int>($"Error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse<int>> UpdateInstructor(InstructorDto instructorDto)
        {
            try
            {
                var instructor = new Instructor
                {
                    Id = instructorDto.InstructorId,
                    Salary = instructorDto.Salary,
                    Certificate = instructorDto.Certificate,
                    Description = instructorDto.Description,
                    TimeFrom = instructorDto.TimeFrom,
                    TimeTo = instructorDto.TimeTo,
                    DaysWork = instructorDto.DaysWork,
                    WorkExperienceFrom = instructorDto.WorkExperienceFrom,
                    WorkExperienceTo = instructorDto.WorkExperienceTo,
                    InsUser_Id = instructorDto.InsUser_Id,
                    YearsOfExperience = CalculatHelper.calculatYearsOfExperience(
                      instructorDto.WorkExperienceTo, instructorDto.WorkExperienceFrom),
                };

                _UnitOfWork.InstructorRepository.Update(instructor);
                await _UnitOfWork.SaveChangesAsync();

                return new ApiResponse<int>(instructor.Id, "Instructor updated successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<int>($"Error occurred: {ex.Message}");
            }
        }


        public async Task<ApiResponse<bool>> DeleteInstructor(int id)
        {
            try
            {
                if (id <= 0)
                    return new ApiResponse<bool>("Instructor not found");

                var instructor = await _UnitOfWork.InstructorRepository.GetByIdAsync(id);

                if (instructor == null)
                    return new ApiResponse<bool>("Instructor not found");

                _UnitOfWork.InstructorRepository.Remove(instructor);
                await _UnitOfWork.SaveChangesAsync();

                return new ApiResponse<bool>(true, "Instructor deleted successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>($"Error occurred: {ex.Message}");
            }
        }
    }
}
