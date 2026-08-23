using DreysFashion.web.Data;
using DreysFashion.web.Models;
using DreysFashion.web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DreysFashion.web.Services.Implementation
{
    /// <summary>
    /// Provides business logic for managing customer
    /// measurement profiles.
    /// </summary>
    public class MeasurementService : IMeasurementService
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MeasurementService"/> class.
        /// </summary>
        public MeasurementService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets the measurement profile belonging to a customer.
        /// </summary>
        public async Task<MeasurementProfile?> GetByUserIdAsync(
            string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return null;
            }

            return await _context.MeasurementProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(measurement =>
                    measurement.UserId == userId);
        }

        /// <summary>
        /// Creates or updates a customer's measurement profile.
        /// </summary>
        public async Task<MeasurementProfile> SaveAsync(
            string userId,
            MeasurementProfile measurement)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new InvalidOperationException(
                    "A valid authenticated user is required.");
            }

            if (measurement == null)
            {
                throw new ArgumentNullException(
                    nameof(measurement));
            }

            // Look for an existing profile.
            var existingProfile =
                await _context.MeasurementProfiles
                    .FirstOrDefaultAsync(profile =>
                        profile.UserId == userId);

            if (existingProfile == null)
            {
                // Create a new profile.
                measurement.Id = 0;
                measurement.UserId = userId;
                measurement.UpdatedAt = DateTime.UtcNow;

                _context.MeasurementProfiles.Add(measurement);
            }
            else
            {
                // Update the existing profile.
                existingProfile.Chest = measurement.Chest;
                existingProfile.Waist = measurement.Waist;
                existingProfile.Hip = measurement.Hip;
                existingProfile.Shoulder = measurement.Shoulder;
                existingProfile.SleeveLength = measurement.SleeveLength;
                existingProfile.TopLength = measurement.TopLength;
                existingProfile.TrouserWaist = measurement.TrouserWaist;
                existingProfile.TrouserLength = measurement.TrouserLength;
                existingProfile.Thigh = measurement.Thigh;
                existingProfile.Knee = measurement.Knee;
                existingProfile.Ankle = measurement.Ankle;

                existingProfile.MeasurementUnit =
                    measurement.MeasurementUnit;

                existingProfile.UpdatedAt = DateTime.UtcNow;

                measurement = existingProfile;
            }

            await _context.SaveChangesAsync();

            return measurement;
        }
    }
}