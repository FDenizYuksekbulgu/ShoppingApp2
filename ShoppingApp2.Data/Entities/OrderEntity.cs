using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShoppingApp2.Data.Entities
{
    public class OrderEntity : BaseEntity
    {
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int CustomerId { get; set; } // Foreign Key

        // Relational Properties
        public UserEntity Customer { get; set; } // Kullanıcıya (Müşteri) ait ilişki
        public ICollection<OrderProductEntity> OrderProducts { get; set; } // Sipariş ile ürünler arasındaki ilişkiyi kuruyor
    }

    public class OrderConfiguration : BaseConfiguration<OrderEntity>
    {
        public override void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
           
            builder.Property(x => x.TotalAmount).IsRequired()
                                         .HasMaxLength(20);

            base.Configure(builder);
        }
    }
}
