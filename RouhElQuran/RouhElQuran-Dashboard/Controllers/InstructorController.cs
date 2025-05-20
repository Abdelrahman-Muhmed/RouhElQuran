using Core.IServices.UserService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using Core.Dto_s;
using RouhElQuran.IServices.CoursesService;


namespace RouhElQuran_Dashboard.Controllers
{
	public class InstructorController : Controller
	{
		private readonly IUserService<Instructor, InstructorDto> _useresService;
		private readonly UserManager<AppUser> _userManager;
		public InstructorController(IUserService<Instructor, InstructorDto> useresService, UserManager<AppUser> userManager)
		{
			_useresService = useresService;
			_userManager = userManager;
		}

		public IActionResult Index() => View();

		public async Task<IActionResult> InstructorHome() 
		{
			var Result = await _useresService.GetAllUser();

			return View(Result);
		}

		//Get By Id
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var result = await _useresService.GetUserById(id);
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
			var instructor = await _useresService.GetUserById(id);
			instructor = id == null ? new InstructorDto() : instructor;
			var instructors = await _userManager.Users
				.Select(user => new
			{
				user.Id,
				FullName = user.FirstName + " " + user.LastName
			})
	        .ToListAsync(); 

			ViewData["AllUser"] = new SelectList(instructors, "Id", "FullName");

			return PartialView("Instructors/_CreateEdite", instructor);

		}

		[HttpPost]
		public async Task<IActionResult> CreateEdit(InstructorDto instructorDto)
		{
			//if (ModelState.IsValid)
			//{
				try
				{
					if (instructorDto.InstructorId != null)
						await _useresService.updateUser(instructorDto);
					else
						await _useresService.CreateUser(instructorDto);


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
					var Result = await _useresService.DeleteUser(id);

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
