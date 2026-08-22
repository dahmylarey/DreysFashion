namespace DreysFashion.web.Models
{
    /// <summary>
    /// Represents a customer's order.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Gets or sets the unique identifier of the order.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the customer's full name.
        /// </summary>
        public string CustomerName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the customer's email address.
        /// </summary>
        public string CustomerEmail { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the customer's phone number.
        /// </summary>
        public string CustomerPhone { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the customer's delivery address.
        /// </summary>
        public string DeliveryAddress { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the total amount of the order.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets the current status of the order.
        /// </summary>
        public string Status { get; set; } = "Pending";

        /// <summary>
        /// Gets or sets the date and time when the order was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the items belonging to this order.
        /// </summary>
        public ICollection<OrderItem> OrderItems { get; set; }
            = new List<OrderItem>();
    }
}