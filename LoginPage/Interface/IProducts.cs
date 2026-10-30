using EyeWearProduct.Model;

namespace EyeWearProduct.Interface
{
    public interface IProducts
    {
        Task<bool> AddProductsAsync(ProductDetailsDto dto);

        Task<List<ProductDetailsDto>> TotalProductAsync();

        Task<string?> UpdateProductAsync(int id, ProductDetails product);

        Task<string?> DeleteProductAsync(int id);
    }
}
