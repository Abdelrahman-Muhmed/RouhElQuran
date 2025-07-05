using Core.HelperModel.FileModel;
using Core.IServices.AboutService;
using Microsoft.AspNetCore.Mvc;

namespace RouhElQuran_Dashboard.Controllers
{

    public class AboutController : Controller
    {
        private readonly  IAboutService _AboutService;
        public AboutController(IAboutService AboutService)
        {
            _AboutService = AboutService;
        }
        public IEnumerable<string> Files { get; set; }
        public IActionResult Index()
        {
            Files = _AboutService.GetAbout();
            return View(Files);
        }

        public IActionResult CreateView()
        {
            return View();
        }

        public IActionResult Create(FileUpload fileUpload)
        {
              _AboutService.CreateAbout(Request , fileUpload);
             
             return RedirectToAction(nameof(Index));
        }

    }
}
