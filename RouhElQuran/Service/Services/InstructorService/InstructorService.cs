using AutoMapper;
using Core.IRepo;
using Core.IServices.InstructorService;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using Service.Dto_s;
using Service.Helper.CalculatHelper;
using Service.Helper.FileUploadHelper;

namespace Service.Services.InstructorService
{
    public class InstructorService :  IInstructorService
    {
        private readonly IGenericrepo<Instructor> _GenericrInstructorepo;

        private readonly IGenericrepo<Files> _fileGenericRepo;

        private readonly IMapper _mapper;
        public InstructorService(IGenericrepo<Instructor> GenericrInstructorepo, IMapper mapper , IGenericrepo<Files> fileGenericRepo) 
        {
            _GenericrInstructorepo = GenericrInstructorepo;
            _mapper = mapper;
            _fileGenericRepo = fileGenericRepo;

        }

        public async Task<IEnumerable<InstructorDto>> GetAllInstructor()
        {
            var result = _GenericrInstructorepo.GetAllAsync();

            var InstructorDto = await result
                .Include(u => u.AppUser)
                .Include(ic => ic.Ins_Courses)
                .ThenInclude(c => c.Course)
                .Select(i => new InstructorDto
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
                  

                }).ToListAsync();


            return InstructorDto;

        }

        public async Task<InstructorDto> GetInstructorById(int? id)
        {
            var result =  _GenericrInstructorepo.GetAllAsync();

            var InstructorDto = await result
               .Where(c => c.Id == id)
               .Include(u => u.AppUser)
               .ThenInclude(u => u.files)
               .Include(ic => ic.Ins_Courses)
               .ThenInclude(c => c.Course)
               .Select(i => new InstructorDto
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
               }).FirstOrDefaultAsync();

            return InstructorDto;

        }
        public async Task<Instructor> CreateInstructor(InstructorDto instructorDto , HttpRequest request)
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
                instructor.YearsOfExperience = CalculatHelper.calculatYearsOfExperience(instructor.WorkExperienceTo , instructor.WorkExperienceFrom);
                var result = await _GenericrInstructorepo.AddAsync(instructor);

                var fileContent = await FileHelper.streamedOrBufferedProcess(request, instructorDto.FileUpload, _fileGenericRepo, userId: instructorDto.InsUser_Id);

                return result;
            }
            catch
            {
                throw;

            }
        }
        public async Task<Instructor> updateInstructor(InstructorDto instructorDto)
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
            var result = await _GenericrInstructorepo.UpdateAsync(instructor);
            return result;
        }
        public async Task<Instructor> DeleteInstructor(int? id)
        {
           var result = await _GenericrInstructorepo.DeleteAsync(id);
            return result;
        }

   

      
    }
}
