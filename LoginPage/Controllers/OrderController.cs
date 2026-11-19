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

            if (result != "Successfuly Order")
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("Get-AllOrders")]
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

        [HttpPut("{orderDetailId}")]

        public async Task<IActionResult> UpdateOrderAsync(int orderDetailId, OrderItemDto dto)
        {
            var updateOrder = await _orderService.UpdateOrderAsync(orderDetailId, dto);

            if(updateOrder != "Successfully updated the order")
            {
                return BadRequest(updateOrder);
            }

            return Ok(updateOrder);
        }
    }
}
