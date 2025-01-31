using AutoMapper;
using Core.IRepo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository.Models;
using Repository.Repos;
using RouhElQuran.AccountService;
using RouhElQuran.PaymentService;
using RouhElQuran.SendEmail;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

namespace RouhElQuran
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Add services to the container

            // Add services to the container.

            builder.Services.AddControllers();
            //Add Authorization UI
            builder.Services.AddSwaggerGen(options =>
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

            builder.Services.AddScoped(typeof(IGenericrepo<>), typeof(Genericrepo<>));
            builder.Services.AddScoped(typeof(ICourseRepository), typeof(CourseRepository));
            builder.Services.AddScoped(typeof(IFreeClassRepository), typeof(FreeClassRepository));
            builder.Services.AddScoped(typeof(IAuthServices), typeof(AuthServices));
            builder.Services.AddScoped(typeof(IPaymentService), typeof(PaymentService.PaymentService));
            builder.Services.AddDbContext<RouhElQuranContext>(Use =>
            Use.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddIdentity<AppUser, IdentityRole<int>>(options => options.SignIn.RequireConfirmedAccount = true)
               .AddEntityFrameworkStores<RouhElQuranContext>().AddDefaultTokenProviders();

            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            builder.Services.AddTransient<IEmailService, EmailService>();
            builder.Services.AddCors(e => e.AddPolicy("Policy", e => e.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

            #endregion Add services to the container

            //I Add Here This Options For AddAuthentication For Return UnAuthorize 401
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters()
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = builder.Configuration["JWT:Issuer"],
                       ValidAudience = builder.Configuration["JWT:Audience"],
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value))
                   };
               });

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            #region Middlewares

            // Configure the HTTP request pipeline
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("Policy");
            app.UseRouting();

            app.UseAuthentication(); // Place authentication before authorization
            app.UseAuthorization();

        

            app.MapControllers();

            app.UseDefaultFiles();
            //app.Use(async (context,next) =>
            //{
            //    await next();
            //    if (context.Response.StatusCode == 404)
            //    {
            //        context.Request.Path = "/index.html";
            //        await next();
            //    }
            //});
            app.UseStaticFiles();
            app.Run();

            #endregion Middlewares
        }
    }
}