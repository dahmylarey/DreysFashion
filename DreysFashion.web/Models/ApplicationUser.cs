using Microsoft.AspNetCore.Identity;

namespace DreysFashion.web.Models
{
    /// <summary>
    /// Represents a customer account in Drey's Fashion.
    /// Extends ASP.NET Core Identity's standard user.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Gets or sets the customer's full name.
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date and time when the account was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}