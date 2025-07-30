using Core.IRepo;
using Core.IServices;
using Core.IServices.AboutService;
using Core.IServices.InstructorCoursesService;
using Core.IServices.InstructorService;
using Core.IServices.UserService;
using Core.IUnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository.Models;
using Repository.Repos;
using Repository.UnitOfWork;
using RouhElQuran.AccountService;
using RouhElQuran.AutoMapper;
using RouhElQuran.IServices.CoursesService;
using RouhElQuran.PaymentServices;
using RouhElQuran.SendEmail;
using Service.Services.AboutService;
using Service.Services.CourcesService;
using Service.Services.InstructorCoursesService;
using Service.Services.InstructorService;
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
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));


            services.AddScoped<IAuthServices, AuthServices>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<ICoursesService, CoursesService>();
            services.AddScoped<IInstructorService, InstructorService>();
            services.AddScoped<IInstructorCoursesService, InstructorCoursesService>();
            services.AddScoped<IAboutService, AboutService>();



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

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                    };
                });

            // Swagger with Authentication Support
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT token like this: Bearer {your token here}"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });


            //-----------------------------------------------MVC 
            //For upload file 
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //// AutoMapper
            //services.AddAutoMapper(typeof(MappingClasses));

        }
    }
}