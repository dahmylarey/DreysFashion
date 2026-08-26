namespace DreysFashion.web.Models
{
    /// <summary>
    /// Represents a customer's custom tailoring request.
    /// </summary>
    public class TailoringRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Identity user ID of the customer.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the associated customer.
        /// </summary>
        public ApplicationUser User { get; set; } = null!;

        /// <summary>
        /// Gets or sets the type of outfit requested.
        /// </summary>
        public string OutfitType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the customer's description of the requested style.
        /// </summary>
        public string StyleDescription { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the fabric preference.
        /// </summary>
        public string? FabricPreference { get; set; }

        /// <summary>
        /// Gets or sets additional instructions from the customer.
        /// </summary>
        public string? AdditionalInstructions { get; set; }

        /// <summary>
        /// Gets or sets the price quoted by the tailor.
        /// </summary>
        public decimal? QuotedAmount { get; set; }

        /// <summary>
        /// Gets or sets the Paystack payment reference
        /// associated with this tailoring request.
        /// </summary>
        public string? PaymentReference { get; set; }

        /// <summary>
        /// Gets or sets the current payment status.
        /// </summary>
        public string PaymentStatus { get; set; } = "Unpaid";

        /// <summary>
        /// Gets or sets the date and time the tailoring request was paid.
        /// </summary>
        public DateTime? PaidAt { get; set; }

        /// <summary>
        /// Gets or sets notes added by the admin.
        /// </summary>
        public string? AdminNotes { get; set; }

        /// <summary>
        /// Gets or sets the current status of the tailoring request.
        /// </summary>
        public string Status { get; set; } = "Pending";

        /// <summary>
        /// Gets or sets the date and time the request was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the date and time the request was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the measurements captured for this request.
        /// </summary>
        public decimal? Chest { get; set; }

        public decimal? Waist { get; set; }

        public decimal? Hip { get; set; }

        public decimal? Shoulder { get; set; }

        public decimal? SleeveLength { get; set; }

        public decimal? TopLength { get; set; }

        public decimal? TrouserWaist { get; set; }

        public decimal? TrouserLength { get; set; }

        public decimal? Thigh { get; set; }

        public decimal? Knee { get; set; }

        public decimal? Ankle { get; set; }

        /// <summary>
        /// Gets or sets the measurement unit used.
        /// </summary>
        public string MeasurementUnit { get; set; } = "inches";
    }
}