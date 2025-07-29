using Core.IRepo;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace RouhElQuran.Controllers
{
   
    public class FreeSessionController : BaseController
    {
        private readonly IFreeClassRepository FreeClassRepo;

        public FreeSessionController(IFreeClassRepository GenericRepo)
        {
            FreeClassRepo = GenericRepo;
        }

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> BookSession(int CourseID)
        {
            if (ModelState.IsValid)
            {
                var ifCourseExceite = await FreeClassRepo.GetCourseByID(CourseID);
                if (ifCourseExceite is null)
                    return NotFound($"Course WithID {CourseID} Not Found");

                if (int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int AppUserId))
                {
                    freeClass freeSession = new freeClass
                    {
                        AppUserId = AppUserId,
                        courseId = CourseID,
                    };

                    await FreeClassRepo.AddAsync(freeSession);
                    return Ok("Session Booked SuccessFully");
                }
                else
                {
                    return BadRequest("Invalid user ID.");
                }


            }
            return BadRequest();
        }

        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> CheckIsTakeSession(int CourseID)
        {
            if (int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int AppUserId))
            {
                var result = await FreeClassRepo.CheckUserAndCourse(AppUserId, CourseID);
                return Ok(result);
            }
            else
            {
                return BadRequest("Invalid user ID.");
            }

        }
    }
}