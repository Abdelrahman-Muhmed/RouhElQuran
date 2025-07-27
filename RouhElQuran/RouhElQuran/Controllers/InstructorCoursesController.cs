using Core.IServices.InstructorCoursesService;
using Microsoft.AspNetCore.Mvc;

namespace RouhElQuran.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class InstructorCoursesController : ControllerBase
	{
		private readonly IInstructorCoursesService _InstructorCoursesService;
		public InstructorCoursesController(IInstructorCoursesService InstructorCoursesService)
		 => _InstructorCoursesService = InstructorCoursesService;

		[HttpGet("GetAll")]
		public IActionResult GetAll()
		{
			var result =  _InstructorCoursesService.GetInstructorCoursesAsync();
			return Ok(result);
		}

	}
}
