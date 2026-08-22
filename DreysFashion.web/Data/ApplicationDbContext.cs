
using DreysFashion.web.Models;
using Microsoft.EntityFrameworkCore;

namespace DreysFashion.web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply ASP.NET Core Identity configuration.
            base.OnModelCreating(modelBuilder);

            // Configure Product.
            modelBuilder.Entity<Product>()
                .Property(product => product.Price)
                .HasPrecision(18, 2);

            // Configure Order.
            modelBuilder.Entity<Order>()
                .Property(order => order.TotalAmount)
                .HasPrecision(18, 2);

            // Configure OrderItem.
            modelBuilder.Entity<OrderItem>()
                .Property(item => item.UnitPrice)
                .HasPrecision(18, 2);

            // Configure Order -> OrderItems relationship.
            modelBuilder.Entity<OrderItem>()
                .HasOne(item => item.Order)
                .WithMany(order => order.OrderItems)
                .HasForeignKey(item => item.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure OrderItem -> Product relationship.
            modelBuilder.Entity<OrderItem>()
                .HasOne(item => item.Product)
                .WithMany()
                .HasForeignKey(item => item.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
