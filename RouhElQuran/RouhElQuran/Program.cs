using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using RouhElQuran.Serivces;
using Service.Helper.FileUploadHelper;

namespace RouhElQuran
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
          

			var builder = WebApplication.CreateBuilder(args);

			// Register Services
			builder.Services.AddAppServices(builder.Configuration);

			// Add MVC and API Controllers
			builder.Services.AddControllersWithViews(); // MVC Controllers
			builder.Services.AddControllers();          // API Controllers

			// Configure Request Limits (e.g., for File Uploads)
			builder.Services.Configure<FormOptions>(options =>
			{
				options.MultipartBodyLengthLimit = 2L * 1024 * 1024 * 1024; // 2 GB
			});

			// Build Application
			var app = builder.Build();

			// Middleware for File Uploads
			app.Use(async (context, next) =>
			{
				context.Features.Get<IHttpMaxRequestBodySizeFeature>().MaxRequestBodySize = 2L * 1024 * 1024 * 1024; // 2 GB
				context.Request.EnableBuffering();
				await next();
			});

			// Configure Middleware
			app.UseHttpsRedirection();
            //app.UseStaticFiles();

            //For Display from Extenal file 
            var staticFilesPathSetting = builder.Configuration["StaticFilesPath"] ?? "wwwroot/Files";

            // Make it absolute
            var staticFilesPath = Path.IsPathRooted(staticFilesPathSetting)
                ? staticFilesPathSetting
                : Path.Combine(builder.Environment.ContentRootPath, staticFilesPathSetting);

            // Create the directory if it does not exist
            if (!Directory.Exists(staticFilesPath))
            {
                Directory.CreateDirectory(staticFilesPath);
                Console.WriteLine($"Created static files directory: {staticFilesPath}");
            }

            // Pass it to FileHelper
            FileHelper.Configure(staticFilesPath);

			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider(staticFilesPath),
				RequestPath = "/files"
			});



			app.UseCors("Policy");
			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();
		

			// Enable Swagger (For API documentation)
		
				app.UseSwagger();
				app.UseSwaggerUI();

			// MVC Routing
			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");
			// Run Application
			app.Run();
		}
    }
}
