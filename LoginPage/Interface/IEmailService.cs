using EyeWearProduct.DTO;
using EyeWearProduct.Model;

namespace EyeWearProduct.Interface
{
    public interface IEmailService
    {
        void SendEmailAsync(EmailDto request);
    }
}
