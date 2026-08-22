using System.Text.Json.Serialization;

namespace DreysFashion.web.ViewModels
{
    /// <summary>
    /// Represents the result of a Paystack transaction verification.
    /// </summary>
    public class PaystackVerificationResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether Paystack
        /// successfully processed the API request.
        /// </summary>
        [JsonPropertyName("status")]
        public bool Status { get; set; }

        /// <summary>
        /// Gets or sets the response message returned by Paystack.
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the verified transaction information.
        /// </summary>
        [JsonPropertyName("data")]
        public PaystackVerificationData? Data { get; set; }
    }

    /// <summary>
    /// Contains verified Paystack transaction information.
    /// </summary>
    public class PaystackVerificationData
    {
        /// <summary>
        /// Gets or sets the transaction status.
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the transaction reference.
        /// </summary>
        [JsonPropertyName("reference")]
        public string Reference { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the amount paid in kobo.
        /// </summary>
        [JsonPropertyName("amount")]
        public long Amount { get; set; }

        /// <summary>
        /// Gets or sets the transaction currency.
        /// </summary>
        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;
    }
}