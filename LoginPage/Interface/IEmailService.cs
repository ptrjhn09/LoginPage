using LoginPage.DTO;

namespace LoginPage.Interface
{
    public interface IEmailService
    {
        void SendEmailAsync(EmailDto request);
    }
}
