using AutoMapper;
using Core.IRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Repository.Models;
using RouhElQuran.AccountService;
using RouhElQuran.Dto_s;
using RouhElQuran.SendEmail;
using System.Security.Claims;

namespace RouhElQuran.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IEmailService emailService;
        private readonly IAuthServices authServices;
        private readonly IGenericrepo<AppUser> GenericRepo;

        public AccountController(UserManager<AppUser> _userManager, IEmailService emaiLservice, IAuthServices _authServices, IGenericrepo<AppUser> genericrepo)
        {
            userManager = _userManager;
            emailService = emaiLservice;
            authServices = _authServices;
            GenericRepo = genericrepo;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto register)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var CheckMail = await userManager.FindByEmailAsync(register.Email);
                    if (CheckMail != null)
                        return BadRequest("This Email is Already Exist");

                    var user = new AppUser()
                    {
                        FirstName = register.FirstName,
                        LastName = register.LastName,
                        Email = register.Email,
                        UserName = register.Email,
                        PasswordHash = register.Password,
                        Language = register.Language,
                        Country = register.Country,
                        PhoneNumber = register.PhoneNumber,
                    };

                    await GenericRepo.BeginTransactionAsync();

                    var result = await userManager.CreateAsync(user, user.PasswordHash);
                    await userManager.AddToRoleAsync(user, register.UserRole ?? "Student");
                    if (result.Succeeded)
                    {
                        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                        var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token }, Request.Scheme);
                        var emailBody = $"Please click the link below to confirm your email: <a>{confirmationLink}</a>";
                        var SendMail = new EmailData
                        {
                            To = register.Email,
                            Subject = "Confirm Your Email",
                            Body = $"Please {emailBody} to confirm your email.",
                        };
                        emailService.SendEmail(SendMail);
                        await GenericRepo.CommitTransactionAsync();
                        return Ok(new { Message = "Register SuccessFully" });
                    }
                    else
                    {
                        var errors = result.Errors.Select(e => e.Description);
                        return BadRequest(errors);
                    }
                }
                catch (Exception ex)
                {
                    await GenericRepo.RollbackTransactionAsync();
                }
            }
            return BadRequest("Invalid Data");
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return BadRequest("User ID or token is invalid.");

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found.");

            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
                return Ok("Email confirmed successfully.");

            return BadRequest("Email confirmation failed.");
        }

        //Login
        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser(LoginDto user)
        {
            if (!ModelState.IsValid)
                return BadRequest("Please Enter Valid Data");

            var GetUser = await authServices.LoginUser(user);
            return Ok(GetUser);
        }

        //Get User Information
        //For test
        [HttpGet, Authorize(Roles = "Student")]
        public ActionResult<object> getUserInfo()
        {
            //var UserName = User?.Identity?.Name;
            //var Name = User.FindFirstValue(ClaimTypes.Name);
            //var Role = User.FindFirstValue(ClaimTypes.Role);
            //return Ok(new { UserName, Name, Role });

            //Other Way using HttpContextAcc
            var userName = emailService.GetName();
            return (userName);
        }
    }
}