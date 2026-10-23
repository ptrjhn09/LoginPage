using LoginPage.Model;

namespace LoginPage.Interface
{
    public interface IProducts
    {
        Task<string?> AddProductsAsync(Product product);

        Task<int> TotalProductAsync();

        Task<string?> UpdateProductAsync(int id, Product product);

        Task<string?> DeleteProductAsync(int id);
    }
}
