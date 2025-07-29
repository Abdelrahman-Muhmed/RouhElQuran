using AutoMapper;
using Core.IRepo;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using RouhElQuran.IServices.CoursesService;
using Service.Dto_s;
using Service.Helper.FileUploadHelper;
namespace Service.Services.CourcesService
{
    public class CoursesService : ICoursesService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IGenericrepo<Files> _fileGenericRepo;
        private readonly ICoursePlanRepository _CoursePlanRepository;
        private readonly IMapper _mapper;
        public CoursesService(ICourseRepository courseRepository, IMapper mapper,
            IGenericrepo<Files> fileGeneRicrepo, ICoursePlanRepository coursePlanRepository)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
            _fileGenericRepo = fileGeneRicrepo;
            _CoursePlanRepository = coursePlanRepository;
        }
        public async Task<CourseDto?> GetCourseById(int? id)
        {

            var result = _courseRepository.GetAllAsync();

            var CourseDto = await result
               .Include(f => f.files)
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
                   Course_Plan = _mapper.Map<List<CoursePlanDto>>(c.CoursePlans),
               }).FirstOrDefaultAsync();
            return CourseDto;
        }

        public async Task<List<CoursePlanDto>> GetCoursePlansByCourseId(int CourseId)
        {

            var result = _CoursePlanRepository.GetAllAsync();

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
            var result = _courseRepository.GetAllAsync();

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

                var Result = await _courseRepository.AddAsync(course);

                var fileContent = await FileHelper.streamedOrBufferedProcess(request, courseDto.FileUpload, _fileGenericRepo, courseId: Result.Id);

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
            var Result = await _courseRepository.UpdateAsync(course);
            return Result;
        }

        public async Task<Course> DeleteCourse(int id)
        {
            var Result = await _courseRepository.DeleteAsync(id);
            return Result;

        }
    }
}
