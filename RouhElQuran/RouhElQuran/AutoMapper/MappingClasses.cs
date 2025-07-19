
using AutoMapper;
using Core.Dto_s;
using Core.HelperModel.FileModel;
using Core.HelperModel.PaginationModel;
using Core.Models;
using Repository.Models;
using System.Linq;

namespace RouhElQuran.AutoMapper
{
    public class MappingClasses : Profile
    {
        public MappingClasses()
        {
            //CreateMap<Course, CourseDto>().ForMember(e => e.Course_Plan, a => a.MapFrom(e => e.CoursePlans)).ReverseMap();

            CreateMap<CoursePlan, CoursePlanDto>().ReverseMap();
            CreateMap<Course, CourseDto>()
            .ForMember(dest => dest.Course_Plan, opt => opt.MapFrom(src => src.CoursePlans));

            CreateMap<CourseDto, Course>()
           .ForMember(dest => dest.CoursePlans, opt => opt.MapFrom(src => src.Course_Plan));



            CreateMap<Files, FileUpload>();
         

            CreateMap<CoursePlan, CoursePlanDto>().ReverseMap();
        
            CreateMap<Student, StudentDto>().ForMember(e => e.FirstName, a => a.MapFrom(e => e.User_id.FirstName))
                .ForMember(e => e.LastName, a => a.MapFrom(e => e.User_id.LastName))
                .ForMember(e => e.PersonalImage, a => a.MapFrom(e => e.User_id.PersonalImage))
                .ForMember(e => e.PhoneNumber, a => a.MapFrom(e => e.User_id.PhoneNumber));
          
            CreateMap<Employee, EmployeeDto>().ForMember(e => e.FirstName, a => a.MapFrom(e => e.User_Id.FirstName))
               .ForMember(e => e.LastName, a => a.MapFrom(e => e.User_Id.LastName))
               .ForMember(e => e.PersonalImage, a => a.MapFrom(e => e.User_Id.PersonalImage))
               .ForMember(e => e.PhoneNumber, a => a.MapFrom(e => e.User_Id.PhoneNumber));
           
            CreateMap<Attendence, AttendenceDto>().ReverseMap();
        }
    }
}