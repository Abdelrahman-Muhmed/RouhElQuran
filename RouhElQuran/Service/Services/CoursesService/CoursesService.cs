using AutoMapper;
using Core.IRepo;
using Core.IUnitOfWork;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
        public async Task<CourseDto?> GetCourseById(int? id)
        {

            var result = _UnitOfWork.CourseRepository.GetAllAsync();

            var CourseDto = await result
               .Include(f => f.files)
               .Include(f => f.CoursePlans)
               .Where(c => c.Id == id)
               .Select(c => new CourseDto
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
                       PlanName = r.Plan.ToString(),
                       PlanNumber = r.Plan,
                       Price = r.Price,
                       SessionCount = r.SessionCount
                   }).ToList(),
               }).FirstOrDefaultAsync();
            return CourseDto;
        }

        public async Task<List<CoursePlanDto>> GetCoursePlansByCourseId(int CourseId)
        {

            var result = _UnitOfWork.CoursePlanRepository.GetAllAsync();

            var CourseDto = await result.Where(c => c.CourseId == CourseId)
               .Select(c => new CoursePlanDto
               {
                   ID = c.ID,
                   CourseId = c.CourseId,
                   PlanName = c.Plan.ToString(),
                   PlanNumber = c.Plan,
                   Price = c.Price,
                   SessionCount = c.SessionCount,
               }).OrderBy(e => e.PlanNumber).ToListAsync();
            return CourseDto;
        }


        public async Task<IEnumerable<CourseDto>> GetAllCourse()
        {
            var result = _UnitOfWork.CourseRepository.GetAllAsync();

            var CourseDto = await result
                .Include(f => f.files)
                .Select(c => new CourseDto
                {
                    Id = c.Id,
                    CourseName = c.CourseName,
                    SessionTime = c.SessionTime,
                    Specialty = c.Specialty,
                    Description = c.Description,
                    CoursesTime = c.CoursesTime,
                    CoursePrice = c.CoursePrice,

                    FileName = c.files.Select(f => f.UntrustedName).ToList()

                }).ToListAsync();

            return CourseDto;
        }


        public async Task<Course> CreateCource(CourseDto courseDto, HttpRequest request)
        {
            try
            {

                Course course = _mapper.Map<Course>(courseDto);

                //TODO: Add The Correct number of plan from enum it come as string from action 
                // Remember must be make it manuel because prop name is plan in model and dto is planNumber
                var Result = await _UnitOfWork.CourseRepository.AddAsync(course);

                var fileContent = await FileHelper.streamedOrBufferedProcess(request, courseDto.FileUpload, _UnitOfWork.FilesRepository, courseId: Result.Id);
                await _UnitOfWork.SaveChangesAsync();

                return Result;
            }
            catch
            {
                throw;

            }
        }



        public async Task<Course> updateCourse(CourseDto courseDto)
        {

            var course = _mapper.Map<Course>(courseDto);
            var Result = await _UnitOfWork.CourseRepository.UpdateAsync(course);
            await _UnitOfWork.SaveChangesAsync();
            return Result;
        }

        public async Task<Course> DeleteCourse(int id)
        {
            var Result = await _UnitOfWork.CourseRepository.DeleteAsync(id);
            await _UnitOfWork.SaveChangesAsync();

            return Result;

        }
    }
}
