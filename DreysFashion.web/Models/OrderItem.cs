namespace DreysFashion.web.Models
{
    /// <summary>
    /// Represents an individual product within an order.
    /// </summary>
    public class OrderItem
    {
        /// <summary>
        /// Gets or sets the unique identifier of the order item.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the order this item belongs to.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Gets or sets the order associated with this item.
        /// </summary>
        public Order Order { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the product.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the product associated with this order item.
        /// </summary>
        public Product Product { get; set; } = null!;

        /// <summary>
        /// Gets or sets the quantity ordered.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the price of one unit at the time of purchase.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets the total price for this order item.
        /// </summary>
        public decimal TotalPrice => UnitPrice * Quantity;


    }
}