using RouhElQuran.AutoMapper;
using Core.Models;
using Repository.Models;
using RouhElQuran.Dto_s;
using AutoMapper;

namespace RouhElQuran.AutoMapper
{
    public class MappingClasses : Profile
    {
        public MappingClasses()
        {
            //CreateMap<Course, CourseDto>().ForMember(e => e.Course_Plan, a => a.MapFrom(e => e.CoursePlans)).ReverseMap();
            ////CreateMap<CoursePlan, CoursePlanDto>().ReverseMap();
            CreateMap<Course, CourseDto>()
            .ForMember(dest => dest.Course_Plan, opt => opt.MapFrom(src => src.CoursePlans))
            .ReverseMap();

            CreateMap<CoursePlan, CoursePlanDto>().ReverseMap();
            ///
            CreateMap<Instructor, InstructorDto>()
                .ForMember(e => e.InsName, a => a.MapFrom(e => e.User_id.FirstName + " " + e.User_id.LastName)).ReverseMap();
            //
            CreateMap<Student, StudentDto>().ForMember(e => e.FirstName, a => a.MapFrom(e => e.User_id.FirstName))
                .ForMember(e => e.LastName, a => a.MapFrom(e => e.User_id.LastName))
                .ForMember(e => e.PersonalImage, a => a.MapFrom(e => e.User_id.PersonalImage))
                .ForMember(e => e.PhoneNumber, a => a.MapFrom(e => e.User_id.PhoneNumber));
            //
            //
            CreateMap<Employee, EmployeeDto>().ForMember(e => e.FirstName, a => a.MapFrom(e => e.User_Id.FirstName))
               .ForMember(e => e.LastName, a => a.MapFrom(e => e.User_Id.LastName))
               .ForMember(e => e.PersonalImage, a => a.MapFrom(e => e.User_Id.PersonalImage))
               .ForMember(e => e.PhoneNumber, a => a.MapFrom(e => e.User_Id.PhoneNumber));
            ///
            CreateMap<Attendence, AttendenceDto>().ReverseMap();
        }
    }
}