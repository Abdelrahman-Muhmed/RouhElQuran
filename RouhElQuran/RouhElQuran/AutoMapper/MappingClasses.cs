
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
            ////CreateMap<CoursePlan, CoursePlanDto>().ReverseMap();
            CreateMap<Course, CourseDto>()
            .ForMember(dest => dest.Course_Plan, opt => opt.MapFrom(src => src.CoursePlans))
            .ReverseMap();

            // Map from Instructor to InstructorDto
            CreateMap<Instructor, InstructorDto>()
                .ForMember(dest => dest.InsName, opt => opt.MapFrom(src => src.User_id.FirstName + " " + src.User_id.LastName))
                .ForMember(dest => dest.InsEmail, opt => opt.MapFrom(src => src.User_id.Email))
                .ForMember(dest => dest.InsUser_Id, opt => opt.MapFrom(src => src.InsUser_Id));
            //.ForPath(dest => dest.User_id, opt => opt.Ignore());

            // Map from InstructorDto to Instructor
            CreateMap<InstructorDto, Instructor>()
                .ForMember(dest => dest.InsUser_Id, opt => opt.MapFrom(src => src.InsUser_Id))
                .ForMember(dest => dest.User_id, opt => opt.Ignore()); // Navigation property, handled by EF

            
            CreateMap<Files, FileUpload>();
            CreateMap<IGrouping<int, Ins_Course>, InstructorCoursesDto>()
                .ForMember(dest => dest.insId, opt => opt.MapFrom(src => src.Key))
                .ForMember(dest => dest.crsIds, opt => opt.MapFrom(src => src.Select(ic => (int?)ic.Course_Id).ToList()))
                .ForMember(dest => dest.instructorDtos, opt => opt.MapFrom(src => src.First().Instructor))
                .ForMember(dest => dest.courseDtos, opt => opt.MapFrom(src => src.Select(ic => ic.Course).ToList()))
                 .ForPath(dest => dest.instructorDtos.FileUpload,opt=> opt.MapFrom(src => src.SelectMany(ic => ic.Instructor.User_id.files).FirstOrDefault()));



            CreateMap<CoursePlan, CoursePlanDto>().ReverseMap();
            ///

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