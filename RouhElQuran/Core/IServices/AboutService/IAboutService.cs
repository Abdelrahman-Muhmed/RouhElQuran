using Core.Dto_s;
using Core.HelperModel.FileModel;
using Microsoft.AspNetCore.Http;
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
        public IEnumerable<string> GetAbout();
        public Task CreateAbout(HttpRequest request, FileUpload fileUpload);
        public Task updateAbout(HttpRequest request);
    }
}
