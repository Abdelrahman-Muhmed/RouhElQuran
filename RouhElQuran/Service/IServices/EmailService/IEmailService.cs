

namespace Core.IServices
{
    public interface IEmailService
    {
        public void SendEmail(EmailData email);

        object GetName();
    }
}