namespace DreysFashion.web.ViewModels
{
    /// <summary>
    /// Represents customer information displayed to administrators.
    /// </summary>
    public class CustomerViewModel
    {
        /// <summary>
        /// Gets or sets the customer's unique identifier.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the customer's full name.
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the customer's email address.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the customer's phone number.
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date the customer created their account.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the number of orders placed by the customer.
        /// </summary>
        public int OrderCount { get; set; }

        /// <summary>
        /// Gets or sets the total amount spent by the customer.
        /// </summary>
        public decimal TotalSpent { get; set; }
    }
}