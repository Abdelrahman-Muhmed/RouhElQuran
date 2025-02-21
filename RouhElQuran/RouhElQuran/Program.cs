using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RouhElQuran.Serivces;

namespace RouhElQuran
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Controllers
            builder.Services.AddControllers();

            // Register Services (Moved to ServiceExtensions.cs)
            builder.Services.AddAppServices(builder.Configuration);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Security & Global Configurations
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseDefaultFiles();
            app.UseCors("Policy");

            // Routing & Authentication/Authorization
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            // Map Endpoints
            app.MapControllers();

            // Run the application
            await app.RunAsync();
        }
    }
}
