using System;
using Microsoft.EntityFrameworkCore;
using ShoppingApp2.Data.Entities;

namespace ShoppingApp2.Data.Context
{
    public class ShoppingApp2DbContext : DbContext
    {
        // Birinci büyük
        public ShoppingApp2DbContext(DbContextOptions<ShoppingApp2DbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fluent API
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderProductConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            modelBuilder.Entity<SettingEntity>().HasData(
                new SettingEntity
                {
                    Id = 1,
                    MaintenenceMode = false
                }
            );

            // User - Order Relationship (UserEntity - OrderEntity)
            modelBuilder.Entity<OrderEntity>()
                .HasOne(o => o.Customer)  // Order'un bir Customer'ı var
                .WithMany(u => u.Orders)  // Bir User birden fazla Order'a sahip
                .HasForeignKey(o => o.CustomerId)  // Foreign Key: CustomerId
                .OnDelete(DeleteBehavior.Restrict);  // Veritabanında silme işlemiyle ilgili davranış (isteğe bağlı)

            // Order - Product Relationship (many-to-many via OrderProduct)
            modelBuilder.Entity<OrderProductEntity>()
                .HasKey(op => new { op.OrderId, op.ProductId }); // Composite Key: OrderId, ProductId

            modelBuilder.Entity<OrderProductEntity>()
                .HasOne(op => op.Order)  // OrderProduct bir Order'a sahip
                .WithMany(o => o.OrderProducts)  // Bir Order birden fazla OrderProduct'a sahip
                .HasForeignKey(op => op.OrderId);  // Foreign Key: OrderId

            modelBuilder.Entity<OrderProductEntity>()
                .HasOne(op => op.Product)  // OrderProduct bir Product'a sahip
                .WithMany(p => p.OrderProducts)  // Bir Product birden fazla OrderProduct'a sahip
                .HasForeignKey(op => op.ProductId);  // Foreign Key: ProductId

            base.OnModelCreating(modelBuilder);
        }

        // DbSet'ler
        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<OrderEntity> Orders => Set<OrderEntity>();
        public DbSet<OrderProductEntity> OrderProducts => Set<OrderProductEntity>();
        public DbSet<ProductEntity> Products => Set<ProductEntity>();
        public DbSet<SettingEntity> Settings => Set<SettingEntity>();
    }
}
