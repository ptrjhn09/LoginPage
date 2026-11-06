using EyeWearProduct.DTO;
using EyeWearProduct.Model;
using CloudinaryDotNet.Actions;

namespace EyeWearProduct.Interface
{
    public interface ICloudinaryService
    {
        Task<CloudinaryResultDto> UploadImageAsync(IFormFile file);
        Task<bool> DeleteImageAsync(string publicId);


    }
}
