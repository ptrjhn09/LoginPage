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
        private readonly ICloudinaryService _cloudinaryService;

        public ProductService(AppDbContext context, Cloudinary cloud, ICloudinaryService cloudinaryService)
        {
            _context = context;
            _cloud = cloud;
            _cloudinaryService = cloudinaryService;
        }



        public async Task<bool> AddProductsAsync(ProductDetailsDto dto)
        {
            var extProduct = await _context.ProductDetails
                .FirstOrDefaultAsync(x => x.ProductName == dto.ProductName);

            if (extProduct != null)
            {
                return false;
            }

               if(dto.Image == null)
            {
                return false;
            }

            var uploadResult =
                await _cloudinaryService.UploadImageAsync(dto.Image);

         

            var productDetails = new ProductDetail
            {
                ProductName = dto.ProductName,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                ImageUrl = uploadResult.ImageUrl,
                CloudinaryPublicId = uploadResult.PublicId,

                CreateDate = DateTime.Now
            };

            _context.ProductDetails.Add(productDetails);
            await _context.SaveChangesAsync();

            var product = new Product
            {
                ProductDetailId = productDetails.ProductDetailId,
                CategoryId = dto.CategoryId,
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

        public async Task<string?> UpdateProductAsync(int productDetailId, ProductDetailsDto dto)
        {
            var existingProducts = await _context.ProductDetails.FindAsync(productDetailId);

            if (existingProducts == null)
            {
                return null;
            }

            if (dto.Image != null)
            {
                await _cloudinaryService.DeleteImageAsync(existingProducts.CloudinaryPublicId);

                var uploadResult = await _cloudinaryService.UploadImageAsync(dto.Image);

                existingProducts.ImageUrl = uploadResult.ImageUrl;

                existingProducts.CloudinaryPublicId = uploadResult.PublicId;
            }


            existingProducts.ProductName = dto.ProductName;
            existingProducts.Price = dto.Price;
            existingProducts.Description = dto.Description;
            existingProducts.Stock = dto.Stock;
            existingProducts.ModifyDate = DateTime.Now;
               

            var prod = await _context.Products.FirstOrDefaultAsync(x => x.ProductDetailId == productDetailId);

            if(prod != null)
            {
                prod.CategoryId = dto.CategoryId;
                prod.ModifyDate = DateTime.Now;
            }

            await _context.SaveChangesAsync();


            return "Succesfully update product";


        }        

        public async Task<string?> DeleteProductAsync(int productDeatildId)
        {
            var Existingproduct = await _context.ProductDetails.FindAsync(productDeatildId);
            if(Existingproduct == null)
            {
                return null;
            }


            await _cloudinaryService .DeleteImageAsync(Existingproduct.CloudinaryPublicId);
            _context.Remove(Existingproduct);
            await _context.SaveChangesAsync();
            return "Product deleted succsesfuly";
        }

    }
}
