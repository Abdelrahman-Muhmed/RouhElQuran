
using Microsoft.AspNetCore.Mvc;
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
                    return View(Result);
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
                if (courseDto == null)
                    return NotFound("Instructor not found");

                return PartialView("Courses/_Details", courseDto);
            }
            catch
            {
                return BadRequest();
            }


        }

        //For get dialog to create or edit
        [HttpGet]
        public async Task<IActionResult> CreateEdit(int? id)
        {
            try
            {
                if (id != null)
                {
                    var result = await _coursesService.GetCourseById(id);
                    return PartialView("Courses/_CreateEdite", result);

                }
                return PartialView("Courses/_CreateEdite", new CourseDto());
            }
            catch
            {
                return BadRequest();
            }



        }

        [HttpPost]
        public async Task<IActionResult> CreateEdit(CourseDto coursedto)
        {
            //if (ModelState.IsValid)
            //{
            try
            {
                if (coursedto.Id == 0)
                    await _coursesService.CreateCource(coursedto, Request);
                else
                    await _coursesService.updateCourse(coursedto);


                return RedirectToAction(nameof(CorsesHome));
            }
            catch
            {
                return BadRequest();
            }
            //}
            //return BadRequest("Invalid Data");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (id > 0)
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
