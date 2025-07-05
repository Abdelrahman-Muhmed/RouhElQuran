
using Core.IRepo;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using Core.Dto_s;
using RouhElQuran.IServices.CoursesService;

namespace RouhElQuran_Dashboard.Controllers
{
	public class CoursesController : Controller
	{
		
		private readonly ICoursesService _coursesService;
     
        public CoursesController(ICoursesService coursesService , IConfiguration configuration)
		{
			_coursesService = coursesService;
        }

		public IActionResult Index() => View();


		[HttpGet("GetAllDash")]
		public async Task<IActionResult> CorsesHome()
		{
			var Result = await _coursesService.GetAllCourse();
			if (Result != null)
				return View(Result);
			else
				return BadRequest();
		}



        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var instructorCorses = await _coursesService.GetCourseById(id);
            if (instructorCorses == null)
                return NotFound("Instructor not found");

            return PartialView("Courses/_Details", instructorCorses);


        }

        //For get dialog to create or edit
        [HttpGet]
		public async Task<IActionResult> CreateEdit(int? id)
		{
			var course = await _coursesService.GetCourseById(id);
		  	course = id == null ? new CourseDto() : course;

            return PartialView("Courses/_CreateEdite", course);

		}

		[HttpPost]
		public async Task<IActionResult> CreateEdit(CourseDto coursedto)
		{
			//if (ModelState.IsValid)
			//{
				try
				{
					if (coursedto.Id == null)
						await _coursesService.CreateCource(coursedto, Request);
					else
						await _coursesService.updateCourse(coursedto);


					return RedirectToAction(nameof(CorsesHome));
				}
				catch
				{
					return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred While Adding");
				}
			//}
			//return BadRequest("Invalid Data");
		}

		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			if (id != null)
			{
				try
				{
					var Result = await _coursesService.DeleteCourse(id);

					if (Result is null)
						return NotFound("Not Found This Course");

					return RedirectToAction(nameof(CorsesHome));
				}
				catch
				{
					return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred While Deleting");
				}
			}
			return BadRequest("Invalid Data");
		}
	}
}
