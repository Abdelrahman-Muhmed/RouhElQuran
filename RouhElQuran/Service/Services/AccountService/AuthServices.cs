using Core.IUnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository.Models;
using Service.Dto_s;
using Service.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RouhElQuran.AccountService
{
    public class AuthServices : ServiceBase, IAuthServices
    {
        private readonly UserManager<AppUser> _UserManager;
        private readonly IConfiguration _Config;
        private readonly SignInManager<AppUser> _SignInManager;

        public AuthServices(UserManager<AppUser> userManager, IConfiguration config, SignInManager<AppUser> _signInManager, IUnitOfWork unitOfWork) 
            : base(unitOfWork)
        {
            _UserManager = userManager;
            _Config = config;
            _SignInManager = _signInManager;
        }

        public async Task<object> LoginUser(LoginDto user)
        {
            var emailCheck = await _UserManager.FindByEmailAsync(user.Email);
            if (emailCheck == null)
                return "Email Or Password Incorrect";

            if (!emailCheck.EmailConfirmed)
                return "Email Not Confirmed";

            var userAfterCheck = await _SignInManager.CheckPasswordSignInAsync(emailCheck, user.Password, false);
            if (!userAfterCheck.Succeeded)
                return "Email Or Password Incorrect";

            return new UserDto()
            {
                Email = emailCheck.Email,
                DisplayName = emailCheck.FirstName,
                Token = await CreateToken(emailCheck)
            };
        }

        public async Task<string> CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            };
            var userRoles = await _UserManager.GetRolesAsync(user);

            foreach (var role in userRoles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Config.GetSection("JWT:Key").Value));

            var signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                issuer: _Config.GetSection("JWT:Issuer").Value,
                audience: _Config.GetSection("JWT:Audience").Value,
                signingCredentials: signingCred);

            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return tokenString;
        }
    }
}