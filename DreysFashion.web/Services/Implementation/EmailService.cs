using System.Net;
using System.Net.Mail;
using DreysFashion.web.Models;
using DreysFashion.web.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace DreysFashion.web.Services
{
    /// <summary>
    /// Provides email sending functionality using SMTP.
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly ILogger<EmailService> _logger;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="EmailService"/> class.
        /// </summary>
        public EmailService(
            IOptions<EmailSettings> settings,
            ILogger<EmailService> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        /// <summary>
        /// Sends an email asynchronously.
        /// </summary>
        public async Task SendAsync(
            string recipient,
            string subject,
            string body,
            bool isHtml = true)
        {
            if (string.IsNullOrWhiteSpace(recipient))
            {
                throw new ArgumentException(
                    "Recipient email address is required.",
                    nameof(recipient));
            }

            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentException(
                    "Email subject is required.",
                    nameof(subject));
            }

            if (string.IsNullOrWhiteSpace(body))
            {
                throw new ArgumentException(
                    "Email body is required.",
                    nameof(body));
            }

            try
            {
                using var message = new MailMessage();

                message.From = new MailAddress(
                    _settings.FromEmail,
                    _settings.FromName);

                message.To.Add(
                    new MailAddress(recipient));

                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = isHtml;

                using var smtpClient = new SmtpClient(
                    _settings.Host,
                    _settings.Port);

                smtpClient.EnableSsl = true;

                smtpClient.Credentials =
                    new NetworkCredential(
                        _settings.Username,
                        _settings.Password);

                await smtpClient.SendMailAsync(message);

                _logger.LogInformation(
                    "Email successfully sent to {Recipient}. Subject: {Subject}",
                    recipient,
                    subject);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to send email to {Recipient}. Subject: {Subject}",
                    recipient,
                    subject);

                throw;
            }
        }
    }
}