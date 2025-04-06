using System.ComponentModel.DataAnnotations;

namespace ShoppingApp2.WebApi.Models
{
    public class UpdateProductRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [Length(5, 100, ErrorMessage = "Product name cannot exceed 100 characters.")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative.")]
        public int StockQuantity { get; set; }
    }
}
