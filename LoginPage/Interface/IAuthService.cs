using LoginPage.DTO;
using LoginPage.Model;

namespace LoginPage.Interface
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(LoginDto request);
        Task<string?> RegisterAsync(RegisterDto request);

        Task<string> GenerateToken(User user);
    }
}
