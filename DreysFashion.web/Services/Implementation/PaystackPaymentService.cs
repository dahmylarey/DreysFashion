using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using DreysFashion.web.Models;
using DreysFashion.web.Services.Interfaces;
using DreysFashion.web.ViewModels;
using Microsoft.Extensions.Options;

namespace DreysFashion.web.Services
{
    /// <summary>
    /// Provides payment processing functionality using Paystack.
    /// </summary>
    public class PaystackPaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly PaystackSettings _settings;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="PaystackPaymentService"/> class.
        /// </summary>
        /// <param name="httpClient">
        /// The HTTP client used to communicate with Paystack.
        /// </param>
        /// <param name="settings">
        /// The Paystack configuration settings.
        /// </param>
        public PaystackPaymentService(
            HttpClient httpClient,
            IOptions<PaystackSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;

            _httpClient.BaseAddress =
                new Uri(_settings.BaseUrl);

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    _settings.SecretKey);
        }

        /// <summary>
        /// Initializes a new Paystack transaction.
        /// </summary>
        /// <param name="email">
        /// The customer's email address.
        /// </param>
        /// <param name="amount">
        /// The transaction amount in Naira.
        /// </param>
        /// <param name="reference">
        /// The unique payment reference.
        /// </param>
        /// <returns>
        /// Paystack transaction information containing the
        /// authorization URL and transaction reference.
        /// </returns>
        public async Task<PaystackTransactionData> InitializePaymentAsync(
            string email,
            decimal amount,
            string reference)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException(
                    "Customer email is required.",
                    nameof(email));
            }

            if (amount <= 0)
            {
                throw new ArgumentException(
                    "Payment amount must be greater than zero.",
                    nameof(amount));
            }

            if (string.IsNullOrWhiteSpace(reference))
            {
                throw new ArgumentException(
                    "Payment reference is required.",
                    nameof(reference));
            }

            // Paystack expects the amount in the smallest
            // currency unit. For NGN, this is Kobo.
            var amountInKobo =
                Convert.ToInt64(amount * 100);

            var request = new
            {
                email = email.Trim(),
                amount = amountInKobo.ToString(),
                currency = "NGN",
                reference = reference,
                callback_url = _settings.CallbackUrl
            };

            var json = JsonSerializer.Serialize(request);

            using var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync(
                "/transaction/initialize",
                content);

            var responseJson =
                await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(
                    $"Paystack returned HTTP {(int)response.StatusCode}: " +
                    responseJson);
            }

            var result =
                JsonSerializer.Deserialize<PaystackInitializeResponse>(
                    responseJson,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (result == null)
            {
                throw new InvalidOperationException(
                    "Paystack returned an empty response.");
            }

            if (!result.Status)
            {
                throw new InvalidOperationException(
                    $"Paystack rejected the transaction: {result.Message}");
            }

            if (result.Data == null ||
                string.IsNullOrWhiteSpace(
                    result.Data.AuthorizationUrl))
            {
                throw new InvalidOperationException(
                    "Paystack did not return a payment authorization URL.");
            }

            return result.Data;
        }

        /// <summary>
        /// Verifies a Paystack transaction using its reference.
        /// </summary>
        /// <param name="reference">
        /// The Paystack transaction reference.
        /// </param>
        /// <returns>
        /// The verified transaction information when successful;
        /// otherwise, null.
        /// </returns>
        public async Task<PaystackVerificationData?> VerifyPaymentAsync(
            string reference)
        {
            if (string.IsNullOrWhiteSpace(reference))
            {
                throw new ArgumentException(
                    "Payment reference is required.",
                    nameof(reference));
            }

            var response = await _httpClient.GetAsync(
                $"/transaction/verify/{reference}");

            var responseJson =
                await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(
                    $"Paystack returned HTTP {(int)response.StatusCode}: " +
                    responseJson);
            }

            var result =
                JsonSerializer.Deserialize<PaystackVerificationResult>(
                    responseJson,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (result == null || !result.Status)
            {
                return null;
            }

            return result.Data;
        }
    }
}