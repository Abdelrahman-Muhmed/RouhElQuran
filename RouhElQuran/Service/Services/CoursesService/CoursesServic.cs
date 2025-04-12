using Core.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore;
using Repository.Repos;
using RouhElQuran.Dto_s;
using RouhElQuran.IServices.CoursesService;
using Repository.Models;

namespace Service.Services.CourcesService
{
	public class CoursesServic : ICoursesService
	{
        private readonly ICourseRepository _courseRepository;
		private readonly IMapper _mapper;

		public CoursesServic(ICourseRepository courseRepository, IMapper mapper)
		{
			_courseRepository = courseRepository;
			_mapper = mapper;

		}
		public async Task<CourseDto> GetCourseById(int? id)
		{
			var getCourse = await _courseRepository.GetCourseWithPlansByIDAsync(id);
			var Result = _mapper.Map<CourseDto>(getCourse);
			return Result;
		}

		public async Task<IEnumerable<CourseDto>> GetAllCourse()
		{
			var Getall = await _courseRepository.GetAllAsync();
			var Result = _mapper.Map<IEnumerable<CourseDto>>(Getall);

			return Result;
		}

		public async Task<Course> CreateCource(CourseDto courseDto)
		{
			Course course = _mapper.Map<Course>(courseDto);
			var Result = await _courseRepository.AddAsync(course);
			return Result;
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
