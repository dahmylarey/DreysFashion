using System.ComponentModel.DataAnnotations;

namespace DreysFashion.web.ViewModels
{
    /// <summary>
    /// Represents the information required to complete a customer checkout.
    /// </summary>
    public class CheckoutViewModel
    {
        /// <summary>
        /// Gets or sets the customer's full name.
        /// </summary>
        [Required(ErrorMessage = "Please enter your full name.")]
        [Display(Name = "Full Name")]
        public string CustomerName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the customer's email address.
        /// </summary>
        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Email Address")]
        public string CustomerEmail { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the customer's phone number.
        /// </summary>
        [Required(ErrorMessage = "Please enter your phone number.")]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        [Display(Name = "Phone Number")]
        public string CustomerPhone { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the customer's delivery address.
        /// </summary>
        [Required(ErrorMessage = "Please enter your delivery address.")]
        [Display(Name = "Delivery Address")]
        public string DeliveryAddress { get; set; } = string.Empty;
    }
}