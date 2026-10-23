using LoginPage.Data;
using LoginPage.Interface;
using LoginPage.Model;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;

namespace LoginPage.Service
{
    public class ProductService:IProducts
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string?> AddProductsAsync(Product product)
        {

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return "Succesfully added the product";
        }

        public async Task<int> TotalProductAsync()
        {
            return await _context.Products.CountAsync();
        }

        public async Task<string?> UpdateProductAsync(int id, Product product)
        {
            var existingProducts = await _context.Products.FindAsync(id);

            if (existingProducts == null)
            {
                return null;
            }

            existingProducts.ProductName = product.ProductName;
            existingProducts.Category = product.Category;
            existingProducts.Price = product.Price;
            existingProducts.Stock = product.Stock;


            await _context.SaveChangesAsync();
            return "Succesfully update product";

          }        

        public async Task<string?> DeleteProductAsync(int id)
        {
            var Existingproduct = await _context.Products.FindAsync(id);
            if(Existingproduct == null)
            {
                return null;
            }

             _context.Remove(Existingproduct);
            await _context.SaveChangesAsync();
            return "Product deleted succsesfuly";
        }

    }
}
