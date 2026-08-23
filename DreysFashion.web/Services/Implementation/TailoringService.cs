using DreysFashion.web.Data;
using DreysFashion.web.Models;
using DreysFashion.web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DreysFashion.web.Services.Implementation
{
    /// <summary>
    /// Provides business logic for customer custom tailoring requests.
    /// </summary>
    public class TailoringService : ITailoringService
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="TailoringService"/> class.
        /// </summary>
        public TailoringService(
            ApplicationDbContext context)
        {
            _context = context;
        }

        // ============================================================
        // CREATE REQUEST
        // ============================================================

        /// <summary>
        /// Creates a new custom tailoring request.
        /// </summary>
        public async Task<TailoringRequest> CreateRequestAsync(
            TailoringRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(
                    nameof(request));
            }

            if (string.IsNullOrWhiteSpace(request.UserId))
            {
                throw new InvalidOperationException(
                    "A valid authenticated user is required.");
            }

            if (string.IsNullOrWhiteSpace(request.OutfitType))
            {
                throw new InvalidOperationException(
                    "Outfit type is required.");
            }

            if (string.IsNullOrWhiteSpace(request.StyleDescription))
            {
                throw new InvalidOperationException(
                    "Style description is required.");
            }

            // Make sure this is always a new request.
            request.Id = 0;

            request.Status = "Pending";
            request.CreatedAt = DateTime.UtcNow;
            request.UpdatedAt = null;
            request.QuotedAmount = null;
            request.AdminNotes = null;

            // We don't need the navigation property when creating.
            request.User = null!;

            _context.TailoringRequests.Add(request);

            await _context.SaveChangesAsync();

            return request;
        }


        // ============================================================
        // GET REQUEST BY ID
        // ============================================================

        /// <summary>
        /// Retrieves a tailoring request by ID.
        /// </summary>
        public async Task<TailoringRequest?> GetRequestByIdAsync(
            int requestId)
        {
            return await _context.TailoringRequests
                .Include(request => request.User)
                .FirstOrDefaultAsync(request =>
                    request.Id == requestId);
        }


        // ============================================================
        // GET CUSTOMER REQUESTS
        // ============================================================

        /// <summary>
        /// Retrieves all tailoring requests belonging
        /// to a specific customer.
        /// </summary>
        public async Task<List<TailoringRequest>> GetRequestsByUserIdAsync(
            string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return new List<TailoringRequest>();
            }

            return await _context.TailoringRequests
                .Where(request =>
                    request.UserId == userId)
                .OrderByDescending(request =>
                    request.CreatedAt)
                .ToListAsync();
        }


        // ============================================================
        // GET ALL REQUESTS
        // ============================================================

        /// <summary>
        /// Retrieves all tailoring requests for administrators.
        /// </summary>
        public async Task<List<TailoringRequest>> GetAllRequestsAsync()
        {
            return await _context.TailoringRequests
                .Include(request => request.User)
                .OrderByDescending(request =>
                    request.CreatedAt)
                .ToListAsync();
        }


        // ============================================================
        // UPDATE STATUS
        // ============================================================

        /// <summary>
        /// Updates the status of a tailoring request.
        /// </summary>
        public async Task<bool> UpdateStatusAsync(
            int requestId,
            string status)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                return false;
            }

            var request = await _context.TailoringRequests
                .FirstOrDefaultAsync(item =>
                    item.Id == requestId);

            if (request == null)
            {
                return false;
            }

            request.Status = status.Trim();
            request.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }


        // ============================================================
        // UPDATE QUOTE
        // ============================================================

        /// <summary>
        /// Updates the quoted amount and administrator notes.
        /// </summary>
        public async Task<bool> UpdateQuoteAsync(
            int requestId,
            decimal quotedAmount,
            string? adminNotes)
        {
            if (quotedAmount <= 0)
            {
                return false;
            }

            var request = await _context.TailoringRequests
                .FirstOrDefaultAsync(item =>
                    item.Id == requestId);

            if (request == null)
            {
                return false;
            }

            request.QuotedAmount = quotedAmount;
            request.AdminNotes = adminNotes?.Trim();
            request.Status = "Quote Provided";
            request.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}