using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using EyeWearProduct.DTO;
using EyeWearProduct.Interface;
using EyeWearProduct.Model;



namespace EyeWearProduct.Service
{
    public class CloudinaryService : ICloudinaryService

    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }


        public async Task<CloudinaryResultDto> UploadImageAsync(IFormFile file)
        {
            if(file == null || file.Length == 0)
            {
                throw new ArgumentException("No image file provided.");
            }

          await using var stream = file.OpenReadStream();

            var ImageploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Width(500).Height(500).Crop("fill")
            };

            var uploadResult = await _cloudinary.UploadAsync(ImageploadParams);


            if (uploadResult.Error != null)
            {
                throw new Exception(uploadResult.Error.Message);
            }

            if (uploadResult.SecureUrl == null)
            {
                throw new Exception("Cloudinary did not return a SecureUrl.");
            }

            return new CloudinaryResultDto
            {
                PublicId = uploadResult.PublicId,
                ImageUrl = uploadResult.SecureUrl.ToString()
            };

        }

        public async Task<bool> DeleteImageAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);

            var result = await _cloudinary.DestroyAsync(deleteParams);

            return result.Result == "ok";
        }
    }
}
    