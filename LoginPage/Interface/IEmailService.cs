using LoginPage.DTO;
using LoginPage.Model;

namespace LoginPage.Interface
{
    public interface IEmailService
    {
        void SendEmailAsync(EmailDto request);
        Task <string?> ForgotPasswordAsync(ForgotPasswordDto forgot);
        Task<string> GeneratePasswordResetToken(User user);
    }
}
