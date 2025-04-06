using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingApp2.Business.Operations.Product.Dtos;
using ShoppingApp2.Data.Entities;

namespace ShoppingApp2.Business.Operations.Order.Dtos
{
    public class OrderDto
    {
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int CustomerId { get; set; } // Foreign Key

        // Relational Properties
        public UserEntity Customer { get; set; } // Kullanıcıya (Müşteri) ait ilişki
        public IEnumerable<ProductDto> OrderProducts { get; set; }
    }
}


