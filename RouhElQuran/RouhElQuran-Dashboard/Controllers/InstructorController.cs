﻿using Core.Dto_s;
using Core.IServices.InstructorCoursesService;
using Core.IServices.InstructorService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using RouhElQuran.IServices.CoursesService;
using System.Threading.Tasks;


namespace RouhElQuran_Dashboard.Controllers
{
	public class InstructorController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly ICoursesService _coursesService;
		private readonly IInstructorService _instructorService;
        private readonly IInstructorCoursesService _instructorCoursesService;

        public InstructorController(
            IInstructorService instructorService,
            UserManager<AppUser> userManager,
			ICoursesService coursesService,
            IInstructorCoursesService instructorCoursesService)
		{
			_userManager = userManager;
			_coursesService = coursesService;
			_instructorService = instructorService;
            _instructorCoursesService = instructorCoursesService;

        }

		public IActionResult Index() => View();

		public async Task<IActionResult> InstructorHome()
		{
            try
            {
                //string sortBy = "Instructor.Salary";
                //bool IsDesc = false;
                //int page = 1;
                //int pageSize = 10;
                var result = await _instructorService.GetAllInstructor();
                return View(result);
            }
            catch
            {
                return BadRequest();
            }
         
		}

        //[HttpPost]
        //public IActionResult InstructorHomeSort(string sortBy, bool IsDesc, int page = 1, int pageSize = 10)
        //{
        //    //var result = _instructorCoursesService.GetInstructorCoursesAsync(sortBy, IsDesc, page, pageSize);

        //    return PartialView("Instructors/_InstructorTCorseTablePartial", result);
        //}

  
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _instructorService.GetInstructorById(id);
                if (result == null)
                    return NotFound("Instructor not found");

                return PartialView("Instructors/_Details", result);
            }

            catch
            {
                return BadRequest();
            }

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
            ViewData["AllCourses"] = new SelectList(courses, "Id", "CourseName");

            if (id != null)
            {
                var result = await _instructorService.GetInstructorById(id);

                return PartialView("Instructors/_CreateEdite", result);

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
                if (instructorDto.InstructorId == null|| instructorDto.InstructorId == 0)
                {
                  

                    var result = await _instructorService.CreateInstructor(instructorDto, Request);

                    InstructorCoursesDto instructorCoursesDto = new InstructorCoursesDto
                    {
                        insId = result.Id,
                        crsIds = instructorDto.CourseIds
                    };
                    await _instructorCoursesService.CreateInstructorCourseAsync(instructorCoursesDto);

                }
                else
                {

                    var result = await _instructorService.updateInstructor(instructorDto);
                    InstructorCoursesDto instructorCoursesDto = new InstructorCoursesDto
                    {
                        insId = result.Id,
                        crsIds = instructorDto.CourseIds
                    };
                    await _instructorCoursesService.UpdateInstructorCourseAsync(instructorCoursesDto);
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
