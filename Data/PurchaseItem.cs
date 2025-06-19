// PurchaseItem.cs
namespace tecnologyApp.Data
{
    public class PurchaseItem
    {
        public int PurchaseId { get; set; } // No debe ser nullable
        public Purchase Purchase { get; set; } = null!;

        public int ProductId { get; set; } // No debe ser nullable
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}

