using Core.Dto_s;
using Core.HelperModel.FileModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices.AboutService
{
    public interface IAboutService 
    {
        IEnumerable<string> GetAbout();
        IActionResult GetSingleFile(string fileName);
        public Task CreateAbout(HttpRequest request, FileUpload fileUpload);
        public Task updateAbout(HttpRequest request);
    }
}
