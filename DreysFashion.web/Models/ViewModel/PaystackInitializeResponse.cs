using System.Text.Json.Serialization;

namespace DreysFashion.web.ViewModels
{
    /// <summary>
    /// Represents the response returned by Paystack
    /// when a transaction is initialized.
    /// </summary>
    public class PaystackInitializeResponse
    {
        /// <summary>
        /// Gets or sets a value indicating whether the request was successful.
        /// </summary>
        [JsonPropertyName("status")]
        public bool Status { get; set; }

        /// <summary>
        /// Gets or sets the response message.
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the transaction response data.
        /// </summary>
        [JsonPropertyName("data")]
        public PaystackTransactionData? Data { get; set; }
    }

    /// <summary>
    /// Contains transaction information returned by Paystack.
    /// </summary>
    public class PaystackTransactionData
    {
        /// <summary>
        /// Gets or sets the URL where the customer completes payment.
        /// </summary>
        [JsonPropertyName("authorization_url")]
        public string AuthorizationUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Paystack access code.
        /// </summary>
        [JsonPropertyName("access_code")]
        public string AccessCode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the unique Paystack transaction reference.
        /// </summary>
        [JsonPropertyName("reference")]
        public string Reference { get; set; } = string.Empty;
    }
}