
using Core.HelperModel.FileModel;
using Core.IServices.AboutService;
using Core.IUnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Helper.FileUploadHelper;

namespace Service.Services.AboutService
{
    public class AboutService : ServiceBase, IAboutService
    {
        private string _StordFilesPath = "D:\\Files";

        public AboutService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public IEnumerable<string> GetAbout()
        {
            if (!Directory.Exists(_StordFilesPath))
                Directory.CreateDirectory(_StordFilesPath);

            var fileNames = Directory.GetFiles(_StordFilesPath)
                           .Select(Path.GetFileName)
                           .Where(name => !string.IsNullOrEmpty(name));


            return fileNames;
        }

        public IActionResult GetSingleFile(string fileName)
        {
            return FileHelper.GetSingleFile(_StordFilesPath, fileName);
        }

        [DisableFormValueModelBinding]
        public async Task CreateAbout(HttpRequest request, FileUpload fileUpload)
        {
            var fileContent = await FileHelper.streamedOrBufferedProcess(request, formFiles: fileUpload);


        }


        public Task updateAbout(HttpRequest request)
        {
            throw new NotImplementedException();
        }

    }
}
