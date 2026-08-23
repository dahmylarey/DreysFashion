using DreysFashion.web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DreysFashion.web.Data
{
    /// <summary>
    /// Represents the application's database context.
    /// Handles products, orders, order items, custom tailoring,
    /// measurements, and ASP.NET Core Identity.
    /// </summary>
    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ApplicationDbContext"/> class.
        /// </summary>
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
        /// Gets or sets the reusable customer measurement profiles.
        /// </summary>
        public DbSet<MeasurementProfile> MeasurementProfiles { get; set; }

        /// <summary>
        /// Gets or sets customer custom tailoring requests.
        /// </summary>
        public DbSet<TailoringRequest> TailoringRequests { get; set; }

        /// <summary>
        /// Configures the application's database relationships,
        /// precision settings, and Identity models.
        /// </summary>
        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            // Apply ASP.NET Core Identity configuration first.
            base.OnModelCreating(modelBuilder);

            // ========================================================
            // PRODUCT
            // ========================================================

            modelBuilder.Entity<Product>()
                .Property(product => product.Price)
                .HasPrecision(18, 2);


            // ========================================================
            // ORDER
            // ========================================================

            modelBuilder.Entity<Order>()
                .Property(order => order.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(item => item.UnitPrice)
                .HasPrecision(18, 2);


            // ========================================================
            // ORDER -> ORDER ITEMS
            // ========================================================

            modelBuilder.Entity<OrderItem>()
                .HasOne(item => item.Order)
                .WithMany(order => order.OrderItems)
                .HasForeignKey(item => item.OrderId)
                .OnDelete(DeleteBehavior.Cascade);


            // ========================================================
            // ORDER ITEM -> PRODUCT
            // ========================================================

            modelBuilder.Entity<OrderItem>()
                .HasOne(item => item.Product)
                .WithMany()
                .HasForeignKey(item => item.ProductId)
                .OnDelete(DeleteBehavior.Restrict);


            // ========================================================
            // ORDER -> USER
            // ========================================================

            modelBuilder.Entity<Order>()
                .HasOne(order => order.User)
                .WithMany()
                .HasForeignKey(order => order.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            // ========================================================
            // MEASUREMENT PROFILE
            // ========================================================

            // Configure measurement precision.
            modelBuilder.Entity<MeasurementProfile>()
                .Property(measurement => measurement.Chest)
                .HasPrecision(8, 2);

            modelBuilder.Entity<MeasurementProfile>()
                .Property(measurement => measurement.Waist)
                .HasPrecision(8, 2);

            modelBuilder.Entity<MeasurementProfile>()
                .Property(measurement => measurement.Hip)
                .HasPrecision(8, 2);

            modelBuilder.Entity<MeasurementProfile>()
                .Property(measurement => measurement.Shoulder)
                .HasPrecision(8, 2);

            modelBuilder.Entity<MeasurementProfile>()
                .Property(measurement => measurement.SleeveLength)
                .HasPrecision(8, 2);

            modelBuilder.Entity<MeasurementProfile>()
                .Property(measurement => measurement.TopLength)
                .HasPrecision(8, 2);

            modelBuilder.Entity<MeasurementProfile>()
                .Property(measurement => measurement.TrouserWaist)
                .HasPrecision(8, 2);

            modelBuilder.Entity<MeasurementProfile>()
                .Property(measurement => measurement.TrouserLength)
                .HasPrecision(8, 2);

            modelBuilder.Entity<MeasurementProfile>()
                .Property(measurement => measurement.Thigh)
                .HasPrecision(8, 2);

            modelBuilder.Entity<MeasurementProfile>()
                .Property(measurement => measurement.Knee)
                .HasPrecision(8, 2);

            modelBuilder.Entity<MeasurementProfile>()
                .Property(measurement => measurement.Ankle)
                .HasPrecision(8, 2);


            // ========================================================
            // MEASUREMENT PROFILE -> USER
            // ========================================================

            modelBuilder.Entity<MeasurementProfile>()
                .HasOne(measurement => measurement.User)
                .WithMany()
                .HasForeignKey(measurement => measurement.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            // ========================================================
            // TAILORING REQUEST
            // ========================================================

            modelBuilder.Entity<TailoringRequest>()
                .Property(request => request.QuotedAmount)
                .HasPrecision(18, 2);


            // Configure tailoring measurement precision.
            modelBuilder.Entity<TailoringRequest>()
                .Property(request => request.Chest)
                .HasPrecision(8, 2);

            modelBuilder.Entity<TailoringRequest>()
                .Property(request => request.Waist)
                .HasPrecision(8, 2);

            modelBuilder.Entity<TailoringRequest>()
                .Property(request => request.Hip)
                .HasPrecision(8, 2);

            modelBuilder.Entity<TailoringRequest>()
                .Property(request => request.Shoulder)
                .HasPrecision(8, 2);

            modelBuilder.Entity<TailoringRequest>()
                .Property(request => request.SleeveLength)
                .HasPrecision(8, 2);

            modelBuilder.Entity<TailoringRequest>()
                .Property(request => request.TopLength)
                .HasPrecision(8, 2);

            modelBuilder.Entity<TailoringRequest>()
                .Property(request => request.TrouserWaist)
                .HasPrecision(8, 2);

            modelBuilder.Entity<TailoringRequest>()
                .Property(request => request.TrouserLength)
                .HasPrecision(8, 2);

            modelBuilder.Entity<TailoringRequest>()
                .Property(request => request.Thigh)
                .HasPrecision(8, 2);

            modelBuilder.Entity<TailoringRequest>()
                .Property(request => request.Knee)
                .HasPrecision(8, 2);

            modelBuilder.Entity<TailoringRequest>()
                .Property(request => request.Ankle)
                .HasPrecision(8, 2);


            // ========================================================
            // TAILORING REQUEST -> USER
            // ========================================================

            modelBuilder.Entity<TailoringRequest>()
                .HasOne(request => request.User)
                .WithMany()
                .HasForeignKey(request => request.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}