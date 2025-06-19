using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using tecnologyApp.Data;
using System.IO;

namespace tecnologyApp.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProductService(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        
        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .Select(p => new Product
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    SellerUsername = p.SellerUsername,
                    Image = p.Image,
                    UserId = p.UserId
                })
                .ToListAsync();
        }

        // Alternative version if you need the User data
        /*
        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.User)
                .Select(p => new Product
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    SellerUsername = p.SellerUsername,
                    Image = p.Image,
                    UserId = p.UserId,
                    User = p.User != null ? new User 
                    {
                        Id = p.User.Id,
                        Username = p.User.Username
                        // Solo incluye las propiedades que necesites
                    } : null
                })
                .ToListAsync();
        }
        */

        public async Task<List<Product>> GetProductsByUserIdAsync(int userId)
        {
            return await _context.Products
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task AddProductAsync(Product product)
        {
            
            if (product.UserId == 0 && !string.IsNullOrEmpty(product.SellerUsername))
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == product.SellerUsername);
                
                if (user != null)
                {
                    product.UserId = user.Id;
                }
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            var existingProduct = await _context.Products.FindAsync(product.Id);
            if (existingProduct != null)
            {
                _context.Entry(existingProduct).CurrentValues.SetValues(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteProductAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Product>> GetProductsByUserAsync(string username)
        {
            return await _context.Products
                .Where(p => p.SellerUsername == username)
                .ToListAsync();
        }

        public async Task<string> UploadProductImage(IBrowserFile file)
        {
            var imagesFolder = Path.Combine(_environment.WebRootPath, "product-images");
            
            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.Name)}";
            var filePath = Path.Combine(imagesFolder, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.OpenReadStream(maxAllowedSize: 5 * 1024 * 1024).CopyToAsync(stream); // Límite de 5MB

            return $"/product-images/{fileName}";
        }

        public async Task UpdateInventoryAfterPurchase(List<Product> purchasedItems)
        {
            foreach (var item in purchasedItems)
            {
                var product = await _context.Products.FindAsync(item.Id);
                if (product != null)
                {
                    product.Quantity -= item.Quantity;
                    if (product.Quantity < 0) product.Quantity = 0;
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<Purchase>> GetPurchaseHistoryAsync(string username)
{
    return await _context.Purchases
        .Where(p => p.User.Username == username)
        .Include(p => p.PurchaseItems)  // Cambiado de Items a PurchaseItems
        .ThenInclude(pi => pi.Product)
        .OrderByDescending(p => p.Date)
        .ToListAsync();
}
    }
}