
using Microsoft.AspNetCore.Mvc;
using Repository.Helper;
using RouhElQuran.IServices.CoursesService;
using Service.Dto_s;

namespace RouhElQuran_Dashboard.Controllers
{
    public class CoursesController : Controller
    {

        private readonly ICoursesService _coursesService;

        public CoursesController(ICoursesService coursesService, IConfiguration configuration)
        {
            _coursesService = coursesService;
        }

        public IActionResult Index() => View();


        [HttpGet("GetAllDash")]
        public async Task<IActionResult> CorsesHome()
        {
            try
            {
                var Result = await _coursesService.GetAllCourse();
                if (Result != null)
                    return View(Result.Data);
                else
                    return BadRequest();

            }
            catch
            {
                return BadRequest();

            }

        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var courseDto = await _coursesService.GetCourseById(id);

                if (courseDto.Data == null)
                    return NotFound(new ApiResponse<CourseDto>("Course not found"));

                return Ok(courseDto.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<CourseDto>($"Unexpected Error: {ex.Message}"));
            }
        }


        //For get dialog to create or edit
        [HttpGet]
        public async Task<IActionResult> CreateEdit(int? id)
        {
            try
            {
                if (id is null)
                    return PartialView("Courses/_CreateEdite", new CourseDto());

                var result = await _coursesService.GetCourseById(id);

                if (result == null)
                    return NotFound(new ApiResponse<CourseDto>("Course not found"));

                return PartialView("Courses/_CreateEdite", result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<CourseDto>($"Unexpected error: {ex.Message}"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateEdit(CourseDto courseDto)
        {
            ModelState.Remove(nameof(courseDto.FileName));
            ModelState.Remove("FileUpload.FormFile");
            for (int i = 0; i < courseDto.Course_Plan.Count; i++)
            {
                ModelState.Remove($"Course_Plan[{i}].PlanName");
            }


            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<CourseDto>("Invalid course data"));

            try
            {
                ApiResponse<bool>? result;

                if (courseDto.Id == 0)
                    result = await _coursesService.CreateCourse(courseDto, Request);
                else
                    result = await _coursesService.UpdateCourse(courseDto);

                if (result is null)
                    return StatusCode(500, new ApiResponse<CourseDto>("No response from service"));

                if (!result.Success)
                    return StatusCode(500, result);

                return RedirectToAction(nameof(CorsesHome));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<CourseDto>($"Unexpected error: {ex.Message}"));
            }
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest(new ApiResponse<string>("Invalid Course ID"));

            var result = await _coursesService.DeleteCourse(id);

            if (!result.Success)
                return NotFound(result);

            return RedirectToAction(nameof(CorsesHome));
        }

    }
}
