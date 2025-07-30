using Core.IServices;
using Core.IUnitOfWork;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using Repository.Models;
using RouhElQuran.SendEmail;
using Service.Services;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using RefreshToken = Core.IServices.RefreshToken;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

public class EmailService : ServiceBase, IEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EmailService(IUnitOfWork unitOfWork, IOptions<EmailSettings> emailSettings, IHttpContextAccessor httpContextAccessor) : base(unitOfWork)
    {
        _emailSettings = emailSettings.Value;
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
        builder.HtmlBody = email.Body;
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