namespace DreysFashion.web.Models
{
    /// <summary>
    /// Contains configuration settings required to communicate with Paystack.
    /// </summary>
    public class PaystackSettings
    {
        /// <summary>
        /// Gets or sets the Paystack secret key.
        /// This key must only be used on the server.
        /// </summary>
        public string SecretKey { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Paystack public key.
        /// </summary>
        public string PublicKey { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Paystack API base URL.
        /// </summary>
        public string BaseUrl { get; set; } = "https://api.paystack.co";

        /// <summary>
        /// Gets or sets the URL where Paystack redirects the customer
        /// after payment.
        /// </summary>
        public string CallbackUrl { get; set; } = string.Empty;
    }
}