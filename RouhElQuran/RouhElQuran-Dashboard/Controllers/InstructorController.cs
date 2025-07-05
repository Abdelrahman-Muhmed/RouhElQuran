using Core.Dto_s;
using Core.IServices.InstructorCoursesService;
using Core.IServices.InstructorService;
using Core.IServices.UserService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using RouhElQuran.IServices.CoursesService;
using Stripe;
using System.Drawing.Printing;
using System.Globalization;


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

		public IActionResult InstructorHome()
		{
            string sortBy = "Instructor.Salary";
            bool IsDesc = false;
            int page = 1;
            int pageSize = 1;
            //var Result =  _instructorCoursesService.GetInstructorCoursesAsync();
            var result = _instructorCoursesService.GetInstructorCoursesAsync(sortBy, IsDesc, page, pageSize);

            return View(result);
		}

        [HttpPost]
        public IActionResult InstructorHomeSort(string sortBy, bool IsDesc,int page = 1 , int pageSize = 10)
        {
            var result = _instructorCoursesService.GetInstructorCoursesAsync(sortBy, IsDesc, page, pageSize);
            return PartialView("Instructors/_InstructorTCorseTablePartial", result);
        }


        [HttpGet]
		public async Task<IActionResult> GetById(int id)
		{
            var instructorCorses = await _instructorCoursesService.GetInstructorCourseByInstructorId(id);
            if (instructorCorses == null)
				return NotFound("Instructor not found");

            return PartialView("Instructors/_Details", instructorCorses);

        
        }

        [HttpGet]
        public async Task<IActionResult> CreateEdit(int? id)
        {
            var courses = await _coursesService.GetAllCourse();
            var instructors = await _userManager.Users
            .Where(x => x.EmailConfirmed == true)
            .Select(user => new
            {
                user.Id,
                FullName = user.FirstName + " " + user.LastName
            })
           .ToListAsync();
            ViewData["AllUser"] = new SelectList(instructors, "Id", "FullName");
            ViewData["AllCourses"] = new SelectList(courses, "Id", "CrsName");

            if (id != null)
            {
                var instructor = await _instructorService.GetInstructorById(id);
                var instructorCorses = await _instructorCoursesService.GetInstructorCourseByInstructorId(id);

                var selectedCourseIds = instructorCorses
                      .SelectMany(c => c.crsIds)
                      .Where(id => id.HasValue)
                      .ToList();
                if (instructor != null)
                    instructor.CourseId = selectedCourseIds;

                return PartialView("Instructors/_CreateEdite", instructor);

            }
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
                {
                    var result = await _instructorService.updateInstructor(instructorDto);
                    InstructorCoursesDto instructorCoursesDto = new InstructorCoursesDto
                    {
                        insId = result.Id,
                        crsIds = instructorDto.CourseId.ToList()
                    };
                    _instructorCoursesService.UpdateInstructorCourseAsync(instructorCoursesDto);

                }
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
        public async Task<IActionResult> Delete(int? id)
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
