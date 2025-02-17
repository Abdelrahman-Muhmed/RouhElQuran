using Microsoft.AspNetCore.Mvc;

namespace RouhElQuran_Dashboard.Controllers
{
	public class InstructorController : Controller
	{
		public IActionResult InstructorHome() => View();

		public IActionResult Index() => View();

	}
}
