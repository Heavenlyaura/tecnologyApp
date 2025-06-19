namespace tecnologyApp.Data;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = "Buyer"; // or "Seller"
    public List<Product> Cart { get; set; } = new List<Product>();
    public List<Purchase> PurchaseHistory { get; set; } = new List<Purchase>();


    public void ClearCart()
    {
        Cart.Clear();
    }


    public void AddToCart(Product product)
    {

        var existingItem = Cart.FirstOrDefault(p => p.Id == product.Id);
        if (existingItem != null)
        {
            existingItem.Quantity += product.Quantity;
        }
        else
        {
            Cart.Add(new Product
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity,
                Image = product.Image,
                SellerUsername = product.SellerUsername
            });
        }
    }


    public void RemoveFromCart(int productId)
    {
        var itemToRemove = Cart.FirstOrDefault(p => p.Id == productId);
        if (itemToRemove != null)
        {
            Cart.Remove(itemToRemove);
        }
    }


    public void AddPurchase(List<Product> items)
    {
        PurchaseHistory.Add(new Purchase
        {
            Date = DateTime.Now,
            Total = items.Sum(item => item.Price * item.Quantity),
            UserId = this.Id,
            PurchaseItems = items.Select(item => new PurchaseItem
            {
                ProductId = item.Id,
                Quantity = item.Quantity,
                UnitPrice = item.Price
            }).ToList()
        });
    }
}


