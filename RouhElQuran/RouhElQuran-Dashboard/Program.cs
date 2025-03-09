using Core.IRepo;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using Repository.Repos;
using RouhElQuran.AutoMapper;
using RouhElQuran.IServices.CoursesService;
using Service.Services.CourcesService;
using Stripe;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
// Register Repositories & Services
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICoursesService, CoursesServic>();
// Build configuration
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();

// Database Context
builder.Services.AddDbContext<RouhElQuranContext>(options =>
	options.UseSqlServer(configuration.GetConnectionString("Connection")));


// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingClasses));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
