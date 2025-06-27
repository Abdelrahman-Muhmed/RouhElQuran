using Core.IServices.UserService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using Core.Dto_s;
using RouhElQuran.IServices.CoursesService;
using Core.IServices.InstructorCoursesService;
using Stripe;
using Core.IServices.InstructorService;


namespace RouhElQuran_Dashboard.Controllers
{
	public class InstructorController : Controller
	{
		//private readonly IUserService<Instructor, InstructorDto> _useresService;
		private readonly UserManager<AppUser> _userManager;
		private readonly ICoursesService _coursesService;
		private readonly IInstructorCoursesService _instructorCoursesService;
		private readonly IInstructorService _instructorService;
		public InstructorController(
            //IUserService<Instructor,InstructorDto> useresService,
            IInstructorService instructorService,
            UserManager<AppUser> userManager,
			ICoursesService coursesService, IInstructorCoursesService instructorCoursesService)
		{
			//_useresService = useresService;
			_userManager = userManager;
			_coursesService = coursesService;
			_instructorCoursesService = instructorCoursesService;
			_instructorService = instructorService;

        }

		public IActionResult Index() => View();

		public async Task<IActionResult> InstructorHome()
		{
			var Result = await _instructorCoursesService.GetInstructorCoursesAsync();
			return View(Result);
		}

		//Get By Id
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var result = await _instructorService.GetInstructorById(id);
			if (result == null)
			{
				return NotFound("Instructor not found");
			}
			return Ok(result);
		}

		//For get dialog to create or edit
		[HttpGet]
		public async Task<IActionResult> CreateEdit(int id)
		{
			var instructor = await _instructorService.GetInstructorById(id);
			instructor = id == null ? new InstructorDto() : instructor;
			var instructors = await _userManager.Users
				.Where(x => x.EmailConfirmed == true)
				.Select(user => new
				{
					user.Id,
					FullName = user.FirstName + " " + user.LastName
				})
			.ToListAsync();

			var courses = await _coursesService.GetAllCourse();
			ViewData["AllUser"] = new SelectList(instructors, "Id", "FullName");
			ViewData["AllCourses"] = new SelectList(courses, "Id", "CrsName");
		
			if (instructor != null)
				return PartialView("Instructors/_CreateEdite", instructor);
			else
				return PartialView("Instructors/_CreateEdite");

		}

		[HttpPost]
		public async Task<IActionResult> CreateEdit(InstructorDto instructorDto)
		{
			//if (ModelState.IsValid)
			//{
			try
			{
				if (instructorDto.Id != null)
					await _instructorService.updateInstructor(instructorDto);
				else
				{
				 var result = await _instructorService.CreateInstructor(instructorDto);
					InstructorCoursesDto instructorCoursesDto = new InstructorCoursesDto
					{
						insId = result.Id,
						crsIds = instructorDto.CourseId.ToList()
					};
			    	await _instructorCoursesService.CreateInstructorCourseAsync(instructorCoursesDto);
				}


				return RedirectToAction(nameof(InstructorHome));
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
					var Result = await _instructorService.DeleteInstructor(id);

					if (Result is null)
						return NotFound("Not Found This Course");

					RedirectToAction("InstructorHome", "Instructor");
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
