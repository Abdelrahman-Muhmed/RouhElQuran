using Core.Dto_s;
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

        public IActionResult Index()
        {
            var files = _AboutService.GetAbout();
            return View(files);
        }

        // Add this method to serve individual files
        public IActionResult GetFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return BadRequest("File name is required");

            var result = _AboutService.GetSingleFile(fileName);
            return result;
        }

   
        [HttpGet]
        public async Task<IActionResult> CreateEdit(int? id)
        {
            //var course = await _AboutService.(id);
            //course = id == null ? new CourseDto() : course;

            return PartialView("About/_CreateEdit");

        }

        [HttpPost]
        public async Task<IActionResult> CreateEdit(FileUpload fileUpload)
        {
           await _AboutService.CreateAbout(Request, fileUpload);

            return RedirectToAction(nameof(Index));
        }


    }
}
