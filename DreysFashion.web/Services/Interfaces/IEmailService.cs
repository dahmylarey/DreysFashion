namespace DreysFashion.web.Services.Interfaces
{
    /// <summary>
    /// Defines operations for sending emails.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends an email asynchronously.
        /// </summary>
        Task SendAsync(
            string recipient,
            string subject,
            string body,
            bool isHtml = true);
    }
}