using Microsoft.AspNetCore.Mvc;

namespace RouhElQuran_Dashboard.Controllers
{
    public class AcademyController : Controller
    {
        public IActionResult AcademyHome() => View();
        

        public IActionResult GetAll() => View();
        
    }
}
