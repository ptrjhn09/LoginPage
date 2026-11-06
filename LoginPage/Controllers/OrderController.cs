using EyeWearProduct.DTO;
using EyeWearProduct.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EyeWearProduct.Controllers
{
    [ApiController]
    [Route("OrderApi")]
    public class OrderController:ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("Add-Order")]
        public async Task<IActionResult> AddOrderAsync(OrderItemDto dto)
        {
            var result = await _orderService.AddOrderAsync(dto);
            if(result == null)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var result = await _orderService.GetAllOrders();

            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderAsync(int id)
        {
            var result = await _orderService.DeleteOrderAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
