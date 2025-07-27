
using Core.IServices.InstructorCoursesService;
using Core.IServices.InstructorService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using RouhElQuran.IServices.CoursesService;
using Service.Dto_s;

namespace RouhElQuran.Controllers
{
    [Route("api/Instructor")]
    [ApiController]
    public class InstructorController : ControllerBase
    {

        //private readonly IUserService<Instructor , InstructorDto> _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ICoursesService _coursesService;
        private readonly IInstructorService _instructorService;
        private readonly IInstructorCoursesService _instructorCoursesService;

        //public InstructorController(IUserService<Instructor, InstructorDto> userService)
        //  =>  _userService = userService;

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

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
                var result = await _instructorService.GetAllInstructor();
            return Ok(result);
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

        //Add New Instructor
        [HttpPost("Add")]
        public async Task<IActionResult> Add(InstructorDto instructorDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _instructorService.CreateInstructor(instructorDto, Request);

                    InstructorCoursesDto instructorCoursesDto = new InstructorCoursesDto
                    {
                        insId = result.Id,
                        crsIds = instructorDto.CourseIds
                    };
                    await _instructorCoursesService.CreateInstructorCourseAsync(instructorCoursesDto);
                    return Ok("Instructor Added Successfully");
                }
                catch
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request");
                }
            }
            return BadRequest("Invalid instructor");
        }

        //Update Instructor
        [HttpPut("Update")]
        public async Task<IActionResult> update(InstructorDto instructorDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _instructorService.updateInstructor(instructorDto);
                    InstructorCoursesDto instructorCoursesDto = new InstructorCoursesDto
                    {
                        insId = result.Id,
                        crsIds = instructorDto.CourseIds
                    };
                    await _instructorCoursesService.UpdateInstructorCourseAsync(instructorCoursesDto);
                    return Ok("Instructor Update successfully");
                }
                catch
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request");
                }
            }
            return BadRequest("Invalid instructor");
        }

        //Remove Instructor
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var Result = await _instructorService.DeleteInstructor(id);
                if (Result != null)
                    return Ok("Instructor Deleted successfully");
                else
                    return NotFound("Instructor not found");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request");
            }
        }
    }
}