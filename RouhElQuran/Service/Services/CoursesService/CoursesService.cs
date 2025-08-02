using AutoMapper;
using Core.IRepo;
using Core.IUnitOfWork;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repository.Helper;
using Repository.Models;
using RouhElQuran.IServices.CoursesService;
using Service.Dto_s;
using Service.Helper.FileUploadHelper;
namespace Service.Services.CourcesService
{
    public class CoursesService : ServiceBase, ICoursesService
    {
        private readonly IMapper _mapper;
        public CoursesService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            _mapper = mapper;
        }
        public async Task<ApiResponse<CourseDto>> GetCourseById(int? id)
        {
            try
            {
                var courseDto = await _UnitOfWork.CourseRepository.SelectFirstOrDefaultAsync(
                     c => c.Id == id,
                    selector: c => new CourseDto
                    {
                        Id = c.Id,
                        CourseName = c.CourseName,
                        SessionTime = c.SessionTime,
                        Specialty = c.Specialty,
                        Description = c.Description,
                        CoursesTime = c.CoursesTime,
                        CoursePrice = c.CoursePrice,
                        FileName = c.files.Select(f => f.UntrustedName).ToList(),
                        Course_Plan = c.CoursePlans.Select(r => new CoursePlanDto
                        {
                            CourseId = r.CourseId,
                            ID = r.ID,
                            PlanName = r.PlanNumber.ToString(),
                            PlanNumber = r.PlanNumber,
                            Price = r.Price,
                            SessionCount = r.SessionCount
                        }).ToList(),
                    },
                    e => e.files,
                    m => m.CoursePlans);

                if (courseDto == null)
                    return new ApiResponse<CourseDto>("Course not found.", false);

                return new ApiResponse<CourseDto>(courseDto, "Course retrieved successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<CourseDto>($"An error occurred: {ex.Message}", false);
            }
        }


        public async Task<List<CoursePlanDto>> GetCoursePlansByCourseId(int CourseId)
        {
            var CourseDto = await _UnitOfWork.CoursePlanRepository.SelectListAsync(c => c.CourseId == CourseId,
                c => new CoursePlanDto
                {
                    ID = c.ID,
                    CourseId = c.CourseId,
                    PlanName = c.PlanNumber.ToString(),
                    PlanNumber = c.PlanNumber,
                    Price = c.Price,
                    SessionCount = c.SessionCount,
                });
            // طريقة جديدة عشان نعمل ترتيب للخطط حسب رقم الخطة
            // بدل ما نستخدم CourseDtoList.OrderBy(e => e.PlanNumber).ToList();
            // قولت اجربها " Mo Salah"
            return [.. CourseDto.OrderBy(e => e.PlanNumber)];
        }

        public async Task<ApiResponse<List<CourseDto>>> GetAllCourse()
        {
            var courseDto = await _UnitOfWork.CourseRepository.SelectListAsync(null,
                c => new CourseDto
                {
                    Id = c.Id,
                    CourseName = c.CourseName,
                    SessionTime = c.SessionTime,
                    Specialty = c.Specialty,
                    Description = c.Description,
                    CoursesTime = c.CoursesTime,
                    CoursePrice = c.CoursePrice,

                    FileName = c.files.Select(f => f.UntrustedName).ToList()

                }, f => f.files);

            return new ApiResponse<List<CourseDto>>(courseDto);
        }

        public async Task<ApiResponse<bool>> CreateCourse(CourseDto courseDto, HttpRequest request)
        {
            try
            {
                Course course = _mapper.Map<Course>(courseDto);
                await _UnitOfWork.CourseRepository.AddAsync(course);
                await _UnitOfWork.SaveChangesAsync(); // نحفظ للحصول على Id قبل رفع الملف

                var fileContent = await FileHelper.streamedOrBufferedProcess(request, courseDto.FileUpload, _UnitOfWork.FilesRepository, courseId: course.Id);

                await _UnitOfWork.SaveChangesAsync();

                return new ApiResponse<bool>("Course created successfully", true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>($"Error: {ex.Message}");
            }
        }


        public async Task<ApiResponse<bool>> UpdateCourse(CourseDto courseDto)
        {
            try
            {
                var course = await _UnitOfWork.CourseRepository.GetByIdAsync(courseDto.Id);
                if (course is null)
                    return new ApiResponse<bool>("Course not found");

                _mapper.Map(courseDto, course); // لتحديث نفس الكيان بدل إنشاء واحد جديد

                _UnitOfWork.CourseRepository.Update(course);
                await _UnitOfWork.SaveChangesAsync();

                return new ApiResponse<bool>(true, "Updated successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>($"Error: {ex.Message}");
            }
        }


        public async Task<ApiResponse<bool>> DeleteCourse(int id)
        {
            try
            {
                var course = await _UnitOfWork.CourseRepository.GetByIdAsync(id);

                if (course is null)
                    return new ApiResponse<bool>("Course not found");

                _UnitOfWork.CourseRepository.Remove(course);
                await _UnitOfWork.SaveChangesAsync();

                return new ApiResponse<bool>(true, "Course deleted successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>($"An error occurred: {ex.Message}");
            }
        }

    }
}
