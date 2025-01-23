using Repository.Models;
using RouhElQuran.Dto_s;

namespace RouhElQuran.SendEmail
{
    public interface IEmailService
    {
        public void SendEmail(EmailData email);

        object GetName();
    }
}