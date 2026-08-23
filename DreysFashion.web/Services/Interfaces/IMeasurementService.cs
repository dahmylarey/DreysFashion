using DreysFashion.web.Models;

namespace DreysFashion.web.Services.Interfaces
{
    /// <summary>
    /// Defines operations for managing customer measurement profiles.
    /// </summary>
    public interface IMeasurementService
    {
        /// <summary>
        /// Gets the measurement profile belonging to a customer.
        /// </summary>
        /// <param name="userId">
        /// The Identity ID of the customer.
        /// </param>
        /// <returns>
        /// The customer's measurement profile if one exists;
        /// otherwise, null.
        /// </returns>
        Task<MeasurementProfile?> GetByUserIdAsync(string userId);

        /// <summary>
        /// Creates a new measurement profile or updates the
        /// existing profile belonging to the customer.
        /// </summary>
        /// <param name="userId">
        /// The Identity ID of the customer.
        /// </param>
        /// <param name="measurement">
        /// The customer's measurements.
        /// </param>
        /// <returns>
        /// The saved measurement profile.
        /// </returns>
        Task<MeasurementProfile> SaveAsync(
            string userId,
            MeasurementProfile measurement);
    }
}