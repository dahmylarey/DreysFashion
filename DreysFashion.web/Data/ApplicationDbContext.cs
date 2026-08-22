using DreysFashion.web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DreysFashion.web.Data
{
    /// <summary>
    /// Represents the application's database context.
    /// Handles products, orders, order items, and ASP.NET Core Identity.
    /// </summary>
    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">
        /// The database context options.
        /// </param>
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the products available in the store.
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Gets or sets the customer orders.
        /// </summary>
        public DbSet<Order> Orders { get; set; }

        /// <summary>
        /// Gets or sets the individual items belonging to orders.
        /// </summary>
        public DbSet<OrderItem> OrderItems { get; set; }

        /// <summary>
        /// Configures the application's database relationships,
        /// precision settings, and Identity models.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder used to configure the database model.
        /// </param>
        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            // Apply ASP.NET Core Identity configuration first.
            base.OnModelCreating(modelBuilder);

            // Configure Product price.
            modelBuilder.Entity<Product>()
                .Property(product => product.Price)
                .HasPrecision(18, 2);

            // Configure Order total amount.
            modelBuilder.Entity<Order>()
                .Property(order => order.TotalAmount)
                .HasPrecision(18, 2);

            // Configure OrderItem unit price.
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


            // Configure Order -> User relationship.
            modelBuilder.Entity<Order>()
    .HasOne(order => order.User)
    .WithMany()
    .HasForeignKey(order => order.UserId)
    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}