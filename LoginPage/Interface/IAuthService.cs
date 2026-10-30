using EyeWearProduct.DTO;
using EyeWearProduct.Model;

namespace EyeWearProduct.Interface
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(LoginDto request);
        Task<string?> RegisterAsync(RegisterDto request);

        Task<string> GenerateToken(User user);

        Task<string?> ForgotPasswordAsync(ForgotPasswordDto forgot);
        Task<string> GeneratePasswordResetToken(User user);

        Task<string?> ResetPassword(ResetPasswordDto reset);

    }
}
