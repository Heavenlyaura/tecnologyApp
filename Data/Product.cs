using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using tecnologyApp.Data;

namespace tecnologyApp.Data
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]  // Key change for database precision
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        public string SellerUsername { get; set; } = string.Empty;

        [Required]
        public string Image { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        public int UserId { get; set; }

        [NotMapped]
        [Range(1, int.MaxValue, ErrorMessage = "You must purchase at least 1 unit")]
        public int QuantityToBuy { get; set; }

        public Product Clone()
        {
            return new Product
            {
                Id = this.Id,
                Name = this.Name,
                Price = this.Price,
                Quantity = this.QuantityToBuy,  // Note: Should this be QuantityToBuy instead of Quantity?
                SellerUsername = this.SellerUsername,
                Image = this.Image,
                UserId = this.UserId
            };
        }
    }
}