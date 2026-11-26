using EyeWearProduct.Data;
using EyeWearProduct.DTO;
using EyeWearProduct.Interface;
using EyeWearProduct.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;


namespace EyeWearProduct.Service
{
    public class TrackingService:ITrackingService
    {
        private readonly AppDbContext _context;

        public TrackingService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string?> AddTrackingAsync(TrackingDto dto)
        {
            var tracking = await _context.TrackingHistory
    .FirstOrDefaultAsync(x => x.OrderDetailId == dto.OrderDetailId);

            if (tracking != null)
            {
                return "Duplicate";
            }

            var newTracking = new TrackingHistory
            {
                Status = "Pending", //Default Status
                OrderDetailId = dto.OrderDetailId,
                StatusDate = DateTime.Now,
                CreatedDate = DateTime.Now
            };

            _context.TrackingHistory.Add(newTracking);

            await _context.SaveChangesAsync();

            return "Successfully track";
        }

        public async Task<List<TrackingHistory>> GetTrackingHistoryAsync()
        {
            return await _context.TrackingHistory.ToListAsync();
        }


    }
}
