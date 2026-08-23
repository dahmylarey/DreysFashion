using System.ComponentModel.DataAnnotations;

namespace DreysFashion.web.ViewModels
{
    /// <summary>
    /// Represents product information used by the application UI.
    /// </summary>
    public class ProductViewModel
    {
        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the product name.
        /// </summary>
        [Required(
            ErrorMessage = "Please enter a product name.")]
        [StringLength(
            150,
            ErrorMessage = "Product name cannot exceed 150 characters.")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the product description.
        /// </summary>
        [Required(
            ErrorMessage = "Please enter a product description.")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the product price.
        /// </summary>
        [Range(
            0.01,
            double.MaxValue,
            ErrorMessage = "Product price must be greater than zero.")]
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the available stock quantity.
        /// </summary>
        [Range(
            0,
            int.MaxValue,
            ErrorMessage = "Stock quantity cannot be negative.")]
        public int StockQuantity { get; set; }

        /// <summary>
        /// Gets or sets the relative URL of the product image.
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets whether the product is available for purchase.
        /// </summary>
        public bool IsAvailable { get; set; }
    }
}