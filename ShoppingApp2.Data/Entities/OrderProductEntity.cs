using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShoppingApp2.Data.Entities
{
    public class OrderProductEntity : BaseEntity
    {
        public int OrderId { get; set; } // Foreign Key
        public int ProductId { get; set; } // Foreign Key
        public int Quantity { get; set; }

        // Relational Properties
        public OrderEntity Order { get; set; } // Sipariş
        public ProductEntity Product { get; set; } // Ürün
    }


    public class OrderProductConfiguration : BaseConfiguration<OrderProductEntity>
    {
        public override void Configure(EntityTypeBuilder<OrderProductEntity> builder)
        {
            builder.Ignore(x => x.Id); // BaseEntity'den gelen Id'yi iptal ettik.
            builder.HasKey(op => new { op.OrderId, op.ProductId }); // Composite Key olarak tanımladık.

            base.Configure(builder);
        }
    }
}
