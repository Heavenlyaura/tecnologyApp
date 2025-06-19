using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tecnologyApp.Data
{
    public class Purchase
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Purchase Date")]  // Translated from "Fecha de Compra"
        public DateTime Date { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "decimal(18,2)")]  // Key configuration for SQL Server
        [Range(0.01, double.MaxValue, ErrorMessage = "Total must be greater than 0")]  // Translated error message
        [Display(Name = "Total")]
        public decimal Total { get; set; }

        [Required]
        [Display(Name = "User")]  // Translated from "Usuario"
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        public ICollection<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();

        /// <summary>
        /// Helper method to automatically calculate the total.
        /// </summary>
        public void CalculateTotal()
        {
            Total = PurchaseItems.Sum(item => item.UnitPrice * item.Quantity);
        }
    }
}