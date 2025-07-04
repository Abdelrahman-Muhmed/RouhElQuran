﻿using Core.IRepo;
using Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using RouhElQuran.AccountService;
using Core.Dto_s;

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
						   //< img src = ""{ 0}"" alt = ""Company Logo"" class=""logo"">
						// Create beautiful email body using the template
						var emailBody = GetEmailConfirmationTemplate(
							firstName: register.FirstName,
							lastName: register.LastName,
							email: register.Email,
							confirmationLink: confirmationLink,
							logoUrl: "https://yourwebsite.com/assets/img/logo.png", 
							companyName: "Rouh-Elquran-Academy" 
						);

						var SendMail = new EmailData
						{
							To = register.Email,
							Subject = "Confirm Your Email - Welcome to Our Platform!",
							Body = emailBody,
						};

						emailService.SendEmail(SendMail);
						await GenericRepo.CommitTransactionAsync();
						return Ok(new { Message = "Register Successfully" });
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
					return StatusCode(500, "An error occurred during registration");
				}
			}
			return BadRequest("Invalid Data");
		}

		private string GetEmailConfirmationTemplate(string firstName, string lastName, string email, string confirmationLink, string logoUrl, string companyName)
		{
			var template = $@"
                    <!DOCTYPE html>
                    <html lang=""en"">
                    <head>
                        <meta charset=""UTF-8"">
                        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                        <title>Confirm Your Email</title>
                        <style>
                            /* Styles omitted for brevity, include your previous styles here */
                        </style>
                    </head>
                    <body>
                        <div class=""email-container"">
                            <div class=""email-header"">
                                <div class=""decorative-icons"">
                                    <div class=""floating-icon icon-1"">✉</div>
                                    <div class=""floating-icon icon-2"">✓</div>
                                    <div class=""floating-icon icon-3"">★</div>
                                    <div class=""floating-icon icon-4"">♦</div>
                                </div>
                                <div class=""logo-container"">
                                    <img src=""{logoUrl}"" alt=""{companyName} Logo"" class=""logo"">
                                </div>
                                <h1>Welcome to Our Platform!</h1>
                                <p>Just one step left to activate your account</p>
                            </div>
            
                            <div class=""email-body"">
                                <div class=""welcome-icon"">✉</div>
                                <h2>Confirm Your Email Address</h2>
                                <p>Hi <strong>{firstName} {lastName}</strong>,</p>
                                <p>Thank you for registering with us! We're excited to have you on board. To complete your registration and secure your account, please confirm your email address by clicking the button below.</p>
                
                                <div class=""user-info"">
                                    <p><strong>Email:</strong> {email}</p>
                                    <p><strong>Registration Date:</strong> {DateTime.Now:MMMM dd, yyyy}</p>
                                </div>
                
                                <a href=""{confirmationLink}"" class=""confirm-button"">Confirm My Email Address</a>
                
                                <div class=""security-note"">
                                    <p><strong>Security Note:</strong> This link will expire in 24 hours for your security. If you didn't create this account, please ignore this email.</p>
                                </div>
                
                                <div class=""alternative-link"">
                                    <p><strong>Having trouble with the button?</strong> Copy and paste this link into your browser:</p>
                                    <a href=""{confirmationLink}"">{confirmationLink}</a>
                                </div>
                            </div>
            
                            <div class=""email-footer"">
                                <p>If you have any questions, feel free to <a href=""mailto:abdelrahmangomaa847@gmail.com"">contact our support team</a>.</p>
                                <p>Best regards,<br>The {companyName} Team</p>
                                <p style=""margin-top: 20px; font-size: 12px;"">This email was sent to {email}. If you received this email by mistake, please ignore it.</p>
                            </div>
                        </div>
                    </body>
                    </html>
			";

			return template;
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
			{
				var htmlResponse = @"
                        <!DOCTYPE html>
                        <html lang='en'>
                        <head>
                            <meta charset='UTF-8'>
                            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                            <title>Email Confirmation</title>
                            <style>
                                :root {
                                    --gradient-bg: linear-gradient(135deg, #667eea, #764ba2);
                                }

                                body {
                                    margin: 0;
                                    font-family: Arial, sans-serif;
                                    display: flex;
                                    justify-content: center;
                                    align-items: center;
                                    min-height: 100vh;
                                    background: var(--gradient-bg);
                                }

                                .confirmation-container {
                                    background: white;
                                    border-radius: 20px;
                                    padding: 50px;
                                    text-align: center;
                                    box-shadow: 0 20px 60px rgba(0, 0, 0, 0.1);
                                    max-width: 400px;
                                    width: 90%;
                                }

                                .confirmation-icon {
                                    width: 80px;
                                    height: 80px;
                                    background: var(--gradient-bg);
                                    border-radius: 50%;
                                    display: flex;
                                    justify-content: center;
                                    align-items: center;
                                    margin: 0 auto 20px;
                                    font-size: 2rem;
                                    color: white;
                                    animation: pulse 2s infinite;
                                }

                                @keyframes pulse {
                                    0%, 100% { transform: scale(1); }
                                    50% { transform: scale(1.1); }
                                }

                                .confirmation-title {
                                    font-size: 1.8rem;
                                    font-weight: bold;
                                    color: #333;
                                    margin-bottom: 20px;
                                }

                                .confirmation-message {
                                    font-size: 1rem;
                                    color: #666;
                                    margin-bottom: 30px;
                                }

                                .confirmation-btn {
                                    display: inline-block;
                                    padding: 10px 20px;
                                    background: var(--gradient-bg);
                                    color: white;
                                    text-decoration: none;
                                    font-weight: bold;
                                    border-radius: 50px;
                                    transition: transform 0.3s, box-shadow 0.3s;
                                }

                                .confirmation-btn:hover {
                                    transform: translateY(-2px);
                                    box-shadow: 0 8px 25px rgba(102, 126, 234, 0.4);
                                }
                            </style>
                            </head>
                            <body>
                                <div class='confirmation-container'>
                                    <div class='confirmation-icon'>&#10003;</div>
                                    <div class='confirmation-title'>Email Confirmed!</div>
                                    <div class='confirmation-message'>
                                        Your email has been confirmed successfully. You can now sign in to your account.
                                    </div>
                                    <a href='https://rouh-el-quran.vercel.app/sign-in' class='confirmation-btn'>Go to Sign In</a>
                                </div>
                            </body>
                            </html>";

				return Content(htmlResponse, "text/html");
			}

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