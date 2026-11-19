using EyeWearProduct.DTO;
using EyeWearProduct.Model;
namespace EyeWearProduct.Interface
{
    public interface IOrderService
    {
        Task <string?> AddOrderAsync(OrderItemDto dto);
        Task<List<OrderItemDto>> GetAllOrders();

        Task<string?> DeleteOrderAsync(int id);

        Task<string?> UpdateOrderAsync(int orderDetailId, OrderItemDto dto);
    }
}
