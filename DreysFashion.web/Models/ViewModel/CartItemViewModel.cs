namespace DreysFashion.web.ViewModels
{
    /// <summary>
    /// Represents a product added to the customer's shopping cart.
    /// </summary>
    public class CartItemViewModel
    {
        /// <summary>
        /// Gets or sets the unique identifier of the product.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the product image URL.
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the price of a single unit of the product.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the product in the cart.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets the total price for this cart item.
        /// </summary>
        public decimal TotalPrice => UnitPrice * Quantity;
    }
}