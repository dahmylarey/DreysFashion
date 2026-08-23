namespace DreysFashion.web.Models
{
    /// <summary>
    /// Stores configuration used by the email service.
    /// </summary>
    public class EmailSettings
    {
        /// <summary>
        /// SMTP server address.
        /// </summary>
        public string Host { get; set; } = string.Empty;

        /// <summary>
        /// SMTP server port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// SMTP username/email address.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// SMTP password or application password.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Email address customers will see as the sender.
        /// </summary>
        public string FromEmail { get; set; } = string.Empty;

        /// <summary>
        /// Name customers will see as the sender.
        /// </summary>
        public string FromName { get; set; } = "Drey's Fashion";

        /// <summary>
        /// Administrator email address that receives
        /// store notifications.
        /// </summary>
        public string AdminEmail { get; set; } = string.Empty;
    }
}