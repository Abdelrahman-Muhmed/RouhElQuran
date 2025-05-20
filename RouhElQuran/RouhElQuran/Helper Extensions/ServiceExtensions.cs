using Core.IRepo;
using Core.IServices;
using Core.IServices.InstructorCoursesService;
using Core.IServices.UserService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository.Models;
using Repository.Repos;
using RouhElQuran.AccountService;
using RouhElQuran.AutoMapper;
using RouhElQuran.IServices.CoursesService;
using RouhElQuran.PaymentServices;
using RouhElQuran.SendEmail;
using Service.Services.CourcesService;
using Service.Services.InstructorCoursesService;
using Service.Services.UserService;
using Swashbuckle.AspNetCore.Filters;
using System.Text;


namespace RouhElQuran.Serivces
{

	public static class ServiceExtensions
    {
        public static void AddAppServices(this IServiceCollection services, IConfiguration configuration)
        {

			// Register Repositories & Services
			services.AddScoped(typeof(IGenericrepo<>), typeof(Genericrepo<>));
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IFreeClassRepository, FreeClassRepository>();
			services.AddScoped<IInstructorCoursesReository, InstructorCoursesReository>();
			services.AddScoped<IAuthServices, AuthServices>();
            services.AddScoped<IPaymentService, PaymentService>();
			services.AddScoped<ICoursesService, CoursesServic>();
            services.AddScoped<IInstructorCoursesService, InstructorCoursesService>();

			services.AddScoped(typeof(IUserService<,>), typeof(UserService<,>));

			// Database Context
			services.AddDbContext<RouhElQuranContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Connection")));

            // Identity & Authentication
            services.AddIdentity<AppUser, IdentityRole<int>>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<RouhElQuranContext>()
                    .AddDefaultTokenProviders();

            // AutoMapper
            services.AddAutoMapper(typeof(Program));

            // Email Configuration
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailService, EmailService>();

            // CORS Policy
            services.AddCors(options =>
                options.AddPolicy("Policy", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

            // JWT Authentication
            var key = Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Key"));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = configuration["JWT:Issuer"],
                            ValidAudience = configuration["JWT:Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(key)
                        };
                    });

            // Swagger with Authentication Support
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });


            //-----------------------------------------------MVC 
            //For upload file 
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			//// AutoMapper
			//services.AddAutoMapper(typeof(MappingClasses));

			


		}
    }
}