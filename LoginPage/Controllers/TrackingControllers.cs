using EyeWearProduct.DTO;
using EyeWearProduct.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EyeWearProduct.Controllers
{

     [ApiController]
    [Route("TrackingApi")]

    public class TrackingControllers: ControllerBase
    {
        private readonly ITrackingService _tracking;

        public TrackingControllers(ITrackingService tracking)
        {
            _tracking = tracking;
        }


        [HttpPost("AddTracking")]
        public async Task<IActionResult> AddTrackingAsync(TrackingDto dto)
        {
            var result = await _tracking.AddTrackingAsync(dto);
            if(result == "Duplicate")
            {
                return BadRequest(result);
            }

            return Ok(result);
        }




    }
}
