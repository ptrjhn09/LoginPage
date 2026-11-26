using EyeWearProduct.DTO;
using EyeWearProduct.Model;
namespace EyeWearProduct.Interface
{
    public interface ITrackingService
    {
        Task<string?> AddTrackingAsync(TrackingDto dto);
        Task<List<TrackingHistory>> GetTrackingHistoryAsync();


        
    }
}
