using Core.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore;
using Repository.Repos;
using Core.Dto_s;
using RouhElQuran.IServices.CoursesService;
using Repository.Models;
using Service.Helper.FileUploadHelper;
using Core.HelperModel;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Stripe.Forwarding;
using Microsoft.Extensions.Configuration;
namespace Service.Services.CourcesService
{
	public class CoursesServic : ICoursesService
	{
		private readonly ICourseRepository _courseRepository;
		private readonly IGenericrepo<Files> _fileGenericRepo;
		private readonly IMapper _mapper;
		//private readonly IConfiguration _configuration;
		private readonly long _fileSizeLimite;
		public CoursesServic(ICourseRepository courseRepository, IMapper mapper,
			IGenericrepo<Files> fileGeneRicrepo)
		{
			_courseRepository = courseRepository;
			_mapper = mapper;
			_fileGenericRepo = fileGeneRicrepo;
			//_configuration = configuration;
			//_fileSizeLimite = long.Parse(_configuration["FileSizeLimite"]);

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
	

		public async Task<Course> CreateCource(CourseDto courseDto, HttpRequest request)
		{


			try
			{
				await _courseRepository.BeginTransactionAsync();

				Course course = _mapper.Map<Course>(courseDto);

				var Result = await _courseRepository.AddAsync(course);

				var fileContent = await FileHelper.streamedOrBufferedProcess( request, courseDto.FileUpload, _fileGenericRepo, courseId:Result.Id);

				await _courseRepository.CommitTransactionAsync();
				return Result;
			}
			catch
			{
				await _courseRepository.RollbackTransactionAsync();
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
