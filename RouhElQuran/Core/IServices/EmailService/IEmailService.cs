using Repository.Models;
using Core.Dto_s;

namespace Core.IServices
{
    public interface IEmailService
    {
        public void SendEmail(EmailData email);

        object GetName();
    }
}