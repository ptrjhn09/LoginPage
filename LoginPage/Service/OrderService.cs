using CloudinaryDotNet.Actions;
using EyeWearProduct.Data;
using EyeWearProduct.DTO;
using EyeWearProduct.Interface;
using EyeWearProduct.Model;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace EyeWearProduct.Service
{
    public class OrderService:IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<string?> AddOrderAsync(OrderItemDto dto)
        {
            var product = await _context.ProductDetails.FirstOrDefaultAsync(x => x.ProductDetailId == dto.ProductDetailId);


            if(product == null)
            {
                return "Product not found";
            }
            if(product.Stock < dto.Quantity)
            {
                return "Insufficent Product";
            }
            if (dto.Quantity <= 0)
            {
                return "Invalid Quantity";
            }

            var totalAmount = product.Price * dto.Quantity;
            var OrderDetails = new OrderDetail
            {
                CustomerName = dto.CustomerName,
                Contact = dto.Contact,
                Address = dto.Address,
                TotalAmount = totalAmount,
                OrderDate = DateTime.Now
                
            };

            product.Stock -= dto.Quantity;
            _context.OrderDetails.Add(OrderDetails);
            await _context.SaveChangesAsync();

            var orders = new Order
            {
                OrderDetailId = OrderDetails.OrderDetailId,
                ProductDetailsId = dto.ProductDetailId,
                Quantity = dto.Quantity,
                CreateDate = DateTime.Now
            };

            _context.Orders.Add(orders);
            await _context.SaveChangesAsync();
            return "Successfuly Order";
        }

        public async Task<List<OrderDetail>> GetAllOrders()
        {
            return await _context.OrderDetails.ToListAsync();
        }

        public async Task<string?> DeleteOrderAsync(int id)
        {
            var order = await _context.OrderDetails
                .FirstOrDefaultAsync(x => x.OrderDetailId == id);

            if (order == null)
            {
                return "Order not found";
            }

            var orderItem = await _context.Orders
                .FirstOrDefaultAsync(x => x.OrderDetailId == id);

            if (orderItem == null)
            {
                return "Order item not found";
            }

            var product = await _context.ProductDetails
                .FirstOrDefaultAsync(x =>
                    x.ProductDetailId == orderItem.ProductDetailsId);

            if (product == null)
            {
                return "Product not found";
            }

            product.Stock += orderItem.Quantity;

            _context.Orders.Remove(orderItem);
            _context.OrderDetails.Remove(order);

            await _context.SaveChangesAsync();

            return "Successfully deleted";
        }



    }
}
