using Microsoft.AspNetCore.Mvc;
using Repository.Helper;
using RouhElQuran.IServices.CoursesService;
using Service.Dto_s;


namespace RouhElQuran.Controllers
{
    public class CoursesController : BaseController
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

        [HttpGet("Plans/{CourseId}")]
        public async Task<IActionResult> GetPlansByCourseId(int CourseId)
        {
            var Result = await _CoursesService.GetCoursePlansByCourseId(CourseId);
            if (Result is null)
                return NotFound($" Course With ID '{CourseId}' Not Found ");
            return Ok(Result);
        }


        [HttpPost("add")]
        public async Task<IActionResult> Create(CourseDto courseDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>("Invalid data", false));

            try
            {
                var result = await _CoursesService.CreateCourse(courseDto, Request);
                return Created(string.Empty, result); // Return 201 with body
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>($"An error occurred while adding: {ex.Message}", false));
            }
        }


        [HttpPut("update")]
        public async Task<IActionResult> UpdateCourse(CourseDto courseDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>("Invalid data", false));

            try
            {
                var result = await _CoursesService.UpdateCourse(courseDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>($"An error occurred while updating: {ex.Message}", false));
            }
        }


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {
                var result = await _CoursesService.DeleteCourse(id);

                if (result == null)
                    return NotFound(new ApiResponse<string>("Course not found", false));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>($"An error occurred while deleting: {ex.Message}", false));
            }
        }

    }
}