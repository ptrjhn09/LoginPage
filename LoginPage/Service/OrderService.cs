using CloudinaryDotNet.Actions;
using CloudinaryDotNet.Core;
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
                CreateDate = DateTime.Now,

             
            };   

            _context.Orders.Add(orders);
            await _context.SaveChangesAsync();

            var tracking = new TrackingHistory
            {
                OrderDetailId = OrderDetails.OrderDetailId,
                Status = "Pending",
                CreatedDate = DateTime.Now,
                StatusDate = DateTime.Now


            };

            _context.TrackingHistory.Add(tracking);
            await _context.SaveChangesAsync();
            return "Successfuly Order";
        }

        public async Task<List<OrderItemDto>> GetAllOrders()
        {
            return await (from o in _context.Orders
                          join od in _context.OrderDetails
                          on o.OrderDetailId equals od.OrderDetailId
                          join pd in _context.ProductDetails
                          on o.ProductDetailsId equals pd.ProductDetailId
                          join th in _context.TrackingHistory
                          on od.OrderDetailId equals th.OrderDetailId
                          select new OrderItemDto
                          {
                              OrderDetailId = od.OrderDetailId,
                              imageUrl = pd.ImageUrl,
                              productName = pd.ProductName,
                              TotalAmount = od.TotalAmount,
                              Quantity = o.Quantity,
                              CustomerName = od.CustomerName,
                              Contact = od.Contact,
                              Address = od.Address,
                              OrderDate = od.OrderDate,
                              Price = pd.Price,
                              Status = th.Status
                          }

                ).ToListAsync();
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

        public async Task<string?> UpdateOrderAsync(int orderDetailId,OrderItemDto dto)
        {
            var existingOrder = await _context.OrderDetails.FindAsync(orderDetailId);

            if (existingOrder == null)
            {
                return "Order detail not found";
            }

            var order = await _context.Orders.FirstOrDefaultAsync(x =>x.OrderDetailId == orderDetailId);

            if (order == null)
            {
                return "Order not found";
            }

            var product = await _context.ProductDetails.FirstOrDefaultAsync(x =>x.ProductDetailId == order.ProductDetailsId);

            if (product == null)
            {
                return "Product not found";
            }

            var difference = dto.Quantity - order.Quantity;
           
            if (difference > 0)
            {
                if (product.Stock < difference)
                {
                    return "Insufficient stock";
                }

                product.Stock -= difference;
            }
            else if (difference < 0)
            {
                product.Stock += Math.Abs(difference);
            }

            existingOrder.CustomerName = dto.CustomerName;
            existingOrder.Contact = dto.Contact;
            existingOrder.Address = dto.Address;

            existingOrder.TotalAmount =
                product.Price * dto.Quantity;

            existingOrder.ModifyDate = DateTime.Now;

            order.Quantity = dto.Quantity;
            order.ModifyDate = DateTime.Now;

            await _context.SaveChangesAsync();

            return "Successfully updated the order";
        }


    }
}
