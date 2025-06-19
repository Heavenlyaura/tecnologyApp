using tecnologyApp.Data;
using Microsoft.AspNetCore.Components.Forms;
namespace tecnologyApp.Services
{
    public interface IProductService
    {
        // Methods for managing products
        Task<List<Product>> GetAllProductsAsync();
        Task<List<Product>> GetProductsByUserIdAsync(int userId);
        Task<Product?> GetProductByIdAsync(int productId);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int productId);
        Task<List<Product>> GetProductsByUserAsync(string username);

        Task<string> UploadProductImage(IBrowserFile file);
        Task UpdateInventoryAfterPurchase(List<Product> purchasedItems);
        Task<List<Purchase>> GetPurchaseHistoryAsync(string username);
        
    }
}
