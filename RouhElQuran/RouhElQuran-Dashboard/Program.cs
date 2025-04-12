using Core.IRepo;
using Core.IServices.UserService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using Repository.Repos;
using RouhElQuran.AutoMapper;
using RouhElQuran.IServices.CoursesService;
using Service.Services.CourcesService;
using Service.Services.UserService;
using Stripe;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
// Register Repositories & Services
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICoursesService, CoursesServic>();

builder.Services.AddScoped(typeof(IGenericrepo<>), typeof(Genericrepo<>));

builder.Services.AddScoped(typeof(IUserService<,>), typeof(UserService<,>));


// Identity & Authentication
builder.Services.AddIdentity<AppUser, IdentityRole<int>>(options => options.SignIn.RequireConfirmedAccount = true)
		.AddEntityFrameworkStores<RouhElQuranContext>()
		.AddDefaultTokenProviders();


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
