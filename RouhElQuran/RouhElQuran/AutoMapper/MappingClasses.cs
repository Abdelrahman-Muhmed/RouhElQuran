
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

			CreateMap<InstructorCoursesDto, List<Ins_Course>>()
				.AfterMap((src, dest) =>
				{
					dest.AddRange(
						src.crsIds
							.Where(courseId => courseId.HasValue) 
							.Select(courseId => new Ins_Course
							{
								Ins_Id = src.insId.Value, 
								Course_Id = courseId.Value 
							})
					);
				});



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