using Core.IRepo;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using RouhElQuran.Dto_s;
using System.Security.Claims;

namespace RouhElQuran.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FreeSessionController : ControllerBase
    {
        private readonly IFreeClassRepository FreeClassRepo;

        public FreeSessionController(IFreeClassRepository GenericRepo)
        {
            FreeClassRepo = GenericRepo;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> BookSession(int CourseID)
        {
            if (ModelState.IsValid)
            {
                var ifCourseExceite = FreeClassRepo.GetCourseByID(CourseID);
                if (ifCourseExceite is null)
                    return NotFound($"Course WithID {CourseID} Not Found");

                freeClass freeSession = new freeClass
                {
                    AppUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    //AppUserId = 2,
                    courseId = CourseID,
                };

                var Result = await FreeClassRepo.AddAsync(freeSession);

                return Ok("Session Booked SuccessFully");
            }
            return BadRequest();
        }

        [HttpGet]
        [Authorize]
        public async Task<bool> CheckIsTakeSession(int CourseID)
        {
            var AppUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            //var AppUserId = 2;

            var result = await FreeClassRepo.CheckUserAndCourse(AppUserId, CourseID);

            return result;
        }
    }
}