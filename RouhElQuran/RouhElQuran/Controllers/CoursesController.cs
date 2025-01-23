using AutoMapper;
using Core.IRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using RouhElQuran.Dto_s;

namespace RouhElQuran.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseRepository CourseRepository;
        private readonly IMapper mapper;

        public CoursesController(ICourseRepository Repository, IMapper _mapper)
        {
            CourseRepository = Repository;
            mapper = _mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var Getall = await CourseRepository.GetAllAsync();
            var Result = mapper.Map<List<CourseDto>>(Getall);

            return Ok(Result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var getCourse = await CourseRepository.GetCourseWithPlansByIDAsync(id);
            if (getCourse is null)
                return NotFound($" Course With ID '{id}' Not Fount ");

            var Result = mapper.Map<CourseDto>(getCourse);
            return Ok(Result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddCourse(CourseDto coursedto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Course course = mapper.Map<Course>(coursedto);
                    await CourseRepository.AddAsync(course);
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
                    Course course = mapper.Map<Course>(coursedto);
                    await CourseRepository.UpdateAsync(course);
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
                    var delete = await CourseRepository.DeleteAsync(id);
                    if (delete is null)
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