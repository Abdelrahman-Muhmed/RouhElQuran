using Microsoft.Extensions.Options;
using RouhElQuran.SendEmail;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Core.Dto_s;
using Microsoft.AspNetCore.Identity;
using Repository.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using Core.IServices;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using RefreshToken = Core.IServices.RefreshToken;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly UserManager<AppUser> _userManager;
    private readonly IConfiguration _config;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EmailService(IOptions<EmailSettings> emailSettings, UserManager<AppUser> userManager, IConfiguration config, IHttpContextAccessor httpContextAccessor)
    {
        _emailSettings = emailSettings.Value;
        _userManager = userManager;
        _config = config;
        _httpContextAccessor = httpContextAccessor;
    }

    public void SendEmail(EmailData email)
    {
        var mail = new MimeMessage
        {
            Sender = MailboxAddress.Parse(_emailSettings.Email),
            Subject = email.Subject
        };

        mail.To.Add(MailboxAddress.Parse(email.To));
        var builder = new BodyBuilder();
        builder.TextBody = email.Body;
        mail.Body = builder.ToMessageBody();
        mail.From.Add(new MailboxAddress(_emailSettings.DisplayName, _emailSettings.Email));

        using var smtp = new SmtpClient();
        smtp.Connect(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
        smtp.Authenticate(_emailSettings.Email, _emailSettings.Password);
        smtp.Send(mail);
        smtp.Disconnect(true);
    }

    //Scond Way ==> Inject HttpContextAccessor
    public object GetName()
    {
        //var result = string.Empty;
        //if(_httpContextAccessor.HttpContext != null)
        //{
        var name = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        var role = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);

        return new { name, role };
        //}
    }

    //Create Refersh Token
    public RefreshToken GetRefreshToken()
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.Now.AddDays(7),
            Created = DateTime.Now
        };

        return refreshToken;
    }

    ////Set Ref Token
    //      private void SetRefreshToken(RefreshToken newRefreshToken)
    //{
    //    var cookieOptions = new CookieOptions
    //    {
    //        HttpOnly = true,
    //        Expires = newRefreshToken.Expires
    //    };
    //    Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

    //    user.RefreshToken = newRefreshToken.Token;
    //    user.TokenCreated = newRefreshToken.Created;
    //    user.TokenExpires = newRefreshToken.Expires;
    //}
}