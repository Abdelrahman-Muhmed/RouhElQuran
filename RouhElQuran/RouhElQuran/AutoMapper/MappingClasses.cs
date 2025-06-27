
using Core.Models;
using Repository.Models;
using Core.Dto_s;
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
            //CreateMap<InstructorCoursesDto, Ins_Course>()
            //        .ForMember(e => e.Ins_Id, a => a.MapFrom(e => e.insId))
            //        .ForMember(e => e.Course_Id, a => a.MapFrom(e => e.crsId))
            //        .ForMember(e => e.Instructor, a => a.MapFrom(e => e.instructorDtos))
            //        .ForMember(e => e.Course, a => a.MapFrom(e => e.courseDtos));

            //CreateMap<InstructorCoursesDto, Ins_Course>()
            //          .ForMember(dest => dest.Ins_Id, opt => opt.MapFrom(src => src.insId))
            //          .ForMember(dest => dest.Course_Id, opt => opt.Ignore()) // Handle this separately in LINQ
            //          .ForMember(dest => dest.Id, opt => opt.Ignore())
            //          .ForMember(dest => dest.Instructor, opt => opt.Ignore())
            //          .ForMember(dest => dest.Course, opt => opt.Ignore());
            // Map from IGrouping<int, Ins_Course> to InstructorCoursesDto



            CreateMap<IGrouping<int, Ins_Course>, InstructorCoursesDto>()
                .ForMember(dest => dest.insId, opt => opt.MapFrom(src => src.Key))
                .ForMember(dest => dest.crsIds, opt => opt.MapFrom(src => src.Select(ic => (int?)ic.Course_Id).ToList()))
                .ForMember(dest => dest.instructorDtos, opt => opt.MapFrom(src => src.First().Instructor))
                .ForMember(dest => dest.courseDtos, opt => opt.MapFrom(src => src.Select(ic => ic.Course).ToList()));


            // Alternative: Direct mapping from Ins_Course collection to single DTO
            CreateMap<IEnumerable<Ins_Course>, InstructorCoursesDto>()
                .ForMember(dest => dest.insId, opt => opt.MapFrom(src => src.First().Ins_Id))
                .ForMember(dest => dest.crsIds, opt => opt.MapFrom(src => src.Select(ic => ic.Course_Id).ToList()))
                .ForMember(dest => dest.instructorDtos, opt => opt.MapFrom(src => src.First().Instructor))
                .ForMember(dest => dest.courseDtos, opt => opt.MapFrom(src => src.Select(ic => ic.Course).ToList()));
            // For Mapping Multiple 
            //CreateMap<InstructorCoursesDto, Ins_Course>();




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