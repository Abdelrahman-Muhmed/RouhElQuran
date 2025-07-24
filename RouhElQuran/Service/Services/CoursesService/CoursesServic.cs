using AutoMapper;
using Core.Dto_s;
using Core.HelperModel;
using Core.IRepo;
using Core.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repository.Models;
using Repository.Repos;
using RouhElQuran.IServices.CoursesService;
using Service.Helper.CalculatHelper;
using Service.Helper.FileUploadHelper;
using Stripe.Forwarding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Service.Services.CourcesService
{
	public class CoursesServic : ICoursesService
	{
		private readonly ICourseRepository _courseRepository;
		private readonly IGenericrepo<Files> _fileGenericRepo;
		private readonly IMapper _mapper;
		public CoursesServic(ICourseRepository courseRepository, IMapper mapper,
			IGenericrepo<Files> fileGeneRicrepo)
		{
			_courseRepository = courseRepository;
			_mapper = mapper;
			_fileGenericRepo = fileGeneRicrepo;

		}
		public async Task<CourseDto> GetCourseById(int? id)
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

                   FileName = c.files.Select(f => f.UntrustedName).ToList()

               }).FirstOrDefaultAsync();
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

				var fileContent = await FileHelper.streamedOrBufferedProcess( request, courseDto.FileUpload, _fileGenericRepo, courseId:Result.Id);

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
