using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingApp2.Data.Enums;

namespace ShoppingApp2.Data.Entities
{
    public class UserEntity : BaseEntity
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public UserType Role { get; set; }

        // Relational Properties
        public ICollection<OrderEntity> Orders { get; set; } // Kullanıcının verdiği siparişleri tutuyor.
    }


    public class UserConfiguration : BaseConfiguration<UserEntity>
    {
        public override void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.Property(x => x.FirstName).IsRequired()
                                              .HasMaxLength(40);

            builder.Property(x => x.LastName).IsRequired()
                                             .HasMaxLength(40);

            base.Configure(builder);
        }
    }
}