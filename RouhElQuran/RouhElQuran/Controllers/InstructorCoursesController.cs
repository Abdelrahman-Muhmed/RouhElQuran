using Core.Dto_s;
using Core.IServices.InstructorCoursesService;
using Microsoft.AspNetCore.Http;
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
		public async Task<IActionResult> GetAll()
		{
			var result = await _InstructorCoursesService.GetInstructorCoursesAsync();
			return Ok(result);
		}

	}
}
