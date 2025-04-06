using ShoppingApp2.Data.Entities;

namespace ShoppingApp2.Business.Operations.Product.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        // Relational Properties
        public ICollection<OrderProductEntity> OrderProducts { get; set; } // Ürünlerin siparişlerle ilişkisi
        public string Title { get; set; }
    }
}