using EyeWearProduct.DTO;
using EyeWearProduct.Model;
namespace EyeWearProduct.Interface
{
    public interface IOrderService
    {
        Task <string?> AddOrderAsync(OrderItemDto dto);
        Task<List<OrderDetail>> GetAllOrders();

        Task<string?> DeleteOrderAsync(int id);
    }
}
