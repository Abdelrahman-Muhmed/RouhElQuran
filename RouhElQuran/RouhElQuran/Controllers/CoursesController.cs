using AutoMapper;
using Core.IRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using Core.Dto_s;
using RouhElQuran.IServices.CoursesService;


namespace RouhElQuran.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICoursesService _CoursesService;

		public CoursesController(ICoursesService CoursesService)
        {
		   _CoursesService = CoursesService;

		}

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
           var Result = await _CoursesService.GetAllCourse();
            return Ok(Result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var Result = await _CoursesService.GetCourseById(id);
            if (Result is null)
                return NotFound($" Course With ID '{id}' Not Fount ");
			return Ok(Result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Create(CourseDto coursedto, HttpRequest request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _CoursesService.CreateCource(coursedto , request);
                    return Created();
                }
                catch
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred While Adding");
                }
            }
            return BadRequest("Invalid Data");
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateCourse(CourseDto coursedto)
        {
            if (ModelState.IsValid)
            {
                try
                {
					await _CoursesService.updateCourse(coursedto);
					return Ok("Update SuccessFully");
                }
                catch
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred While Updating");
                }
            }
            return BadRequest("Invalid Data");
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var Result = _CoursesService.DeleteCourse(id);

					if (Result is null)
                        return NotFound("Not Found This Course");

                    return Ok("Deleted SuccessFully");
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