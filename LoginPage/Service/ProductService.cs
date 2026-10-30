using EyeWearProduct.Data;
using EyeWearProduct.Interface;
using EyeWearProduct.Model;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System.Xml;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using dotenv.net;

namespace EyeWearProduct.Service
{
    public class ProductService:IProducts
    {
        private readonly AppDbContext _context;
        private readonly Cloudinary _cloud;

        public ProductService(AppDbContext context, Cloudinary cloud)
        {
            _context = context;
            _cloud = cloud;

        }



        public async Task<bool> AddProductsAsync(ProductDetailsDto dto)
        {

            var extProduct = await _context.ProductDetails.FirstOrDefaultAsync(x => x.ProductName == dto.ProductName);

            if (extProduct != null)
            {
                return false;
            }

            var ProductDetails = new ProductDetails
            {
                ProductName = dto.ProductName,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                ImageUrl = dto.ImageUrl,
                CreateDate = DateTime.Now
            };

            _context.ProductDetails.Add(ProductDetails);
            await _context.SaveChangesAsync();


            var product = new Product
            {
                ProductDetailId = ProductDetails.ProductDetailId,
                CategoryId = dto.CategoryId,//assign value in category section FE
                CreateDate = DateTime.Now

            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();


            return true;
        }

        public async Task<List<ProductDetailsDto>> TotalProductAsync()
        {
            return await (from p in _context.Products
                          join pd in _context.ProductDetails
                         on p.ProductDetailId equals pd.ProductDetailId
                          join c in _context.Categories
                         on p.CategoryId equals c.CategoryId

                          select new ProductDetailsDto
                          {
                              ProductDetailId = pd.ProductDetailId,
                              ProductName = pd.ProductName,
                              Description = pd.Description,
                              Price = pd.Price,
                              Stock = pd.Stock,
                              ProductCategory = c.ProductCategory,
                              ImageUrl = pd.ImageUrl

                          }
                          ).ToListAsync();
            
        }

        public async Task<string?> UpdateProductAsync(int id, ProductDetails product)
        {
            var existingProducts = await _context.ProductDetails.FindAsync(id);

            if (existingProducts == null)
            {
                return null;
            }


            existingProducts.ProductName = product.ProductName;
            //existingProducts.Category = product.Category;
            existingProducts.Price = product.Price;
            existingProducts.Stock = product.Stock;

            existingProducts.ModifyDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return "Succesfully update product";

          }        

        public async Task<string?> DeleteProductAsync(int id)
        {
            var Existingproduct = await _context.ProductDetails.FindAsync(id);
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
