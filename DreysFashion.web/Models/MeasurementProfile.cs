namespace DreysFashion.web.Models
{
    /// <summary>
    /// Stores a customer's reusable body measurements
    /// for custom tailoring orders.
    /// </summary>
    public class MeasurementProfile
    {
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Identity user ID who owns
        /// this measurement profile.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the associated customer.
        /// </summary>
        public ApplicationUser User { get; set; } = null!;

        /// <summary>
        /// Gets or sets the chest measurement.
        /// </summary>
        public decimal? Chest { get; set; }

        /// <summary>
        /// Gets or sets the waist measurement.
        /// </summary>
        public decimal? Waist { get; set; }

        /// <summary>
        /// Gets or sets the hip measurement.
        /// </summary>
        public decimal? Hip { get; set; }

        /// <summary>
        /// Gets or sets the shoulder measurement.
        /// </summary>
        public decimal? Shoulder { get; set; }

        /// <summary>
        /// Gets or sets the sleeve measurement.
        /// </summary>
        public decimal? SleeveLength { get; set; }

        /// <summary>
        /// Gets or sets the shirt/top length measurement.
        /// </summary>
        public decimal? TopLength { get; set; }

        /// <summary>
        /// Gets or sets the trouser waist measurement.
        /// </summary>
        public decimal? TrouserWaist { get; set; }

        /// <summary>
        /// Gets or sets the trouser length measurement.
        /// </summary>
        public decimal? TrouserLength { get; set; }

        /// <summary>
        /// Gets or sets the thigh measurement.
        /// </summary>
        public decimal? Thigh { get; set; }

        /// <summary>
        /// Gets or sets the knee measurement.
        /// </summary>
        public decimal? Knee { get; set; }

        /// <summary>
        /// Gets or sets the ankle measurement.
        /// </summary>
        public decimal? Ankle { get; set; }

        /// <summary>
        /// Gets or sets the unit used for the measurements.
        /// Example: inches or centimeters.
        /// </summary>
        public string MeasurementUnit { get; set; } = "inches";

        /// <summary>
        /// Gets or sets the date the measurements were last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}