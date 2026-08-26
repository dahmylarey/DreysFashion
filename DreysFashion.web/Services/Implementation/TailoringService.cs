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
        private readonly IEmailService _emailService;

        // Change this if your admin email is different.
        private const string AdminEmail =
            "oladeleoluwada@gmail.com";

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="TailoringService"/> class.
        /// </summary>
        public TailoringService(
            ApplicationDbContext context,
            IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }


        // ============================================================
        // CREATE REQUEST
        // ============================================================

        /// <summary>
        /// Creates a new custom tailoring request and sends
        /// notifications to the customer and administrator.
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

            // --------------------------------------------------------
            // Make sure this is always a new request.
            // --------------------------------------------------------

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


            // --------------------------------------------------------
            // Get customer information from the database.
            // --------------------------------------------------------

            var customer = await _context.Users
                .FirstOrDefaultAsync(user =>
                    user.Id == request.UserId);


            // --------------------------------------------------------
            // CUSTOMER NOTIFICATION
            // --------------------------------------------------------

            if (customer != null &&
                !string.IsNullOrWhiteSpace(customer.Email))
            {
                try
                {
                    var customerName =
                        string.IsNullOrWhiteSpace(customer.FullName)
                            ? "Customer"
                            : customer.FullName;

                    var customerBody = $@"
<html>
<body style=""font-family:Arial,sans-serif;"">

    <h2>Tailoring Request Received</h2>

    <p>
        Hello <strong>{customerName}</strong>,
    </p>

    <p>
        Thank you for submitting a custom tailoring request
        to <strong>Drey's Fashion</strong>.
    </p>

    <p>
        We have successfully received your request and
        our team will review it shortly.
    </p>

    <hr />

    <p>
        <strong>Request Number:</strong><br />
        #{request.Id}
    </p>

    <p>
        <strong>Outfit Type:</strong><br />
        {request.OutfitType}
    </p>

    <p>
        <strong>Style Description:</strong><br />
        {request.StyleDescription}
    </p>

    <p>
        <strong>Fabric Preference:</strong><br />
        {request.FabricPreference ?? "Not specified"}
    </p>

    <p>
        <strong>Status:</strong><br />
        Pending
    </p>

    <hr />

    <p>
        We will contact you when there is an update
        regarding your request.
    </p>

    <p>
        <strong>Drey's Fashion</strong>
    </p>

</body>
</html>";

                    await _emailService.SendAsync(
                        customer.Email,
                        $"Tailoring Request Received - #{request.Id}",
                        customerBody,
                        true);
                }
                catch
                {
                    // Email failure must not prevent
                    // the tailoring request from being created.
                }
            }


            // --------------------------------------------------------
            // ADMIN NOTIFICATION
            // --------------------------------------------------------

            try
            {
                var customerName =
                    customer?.FullName ?? "Unknown Customer";

                var customerEmail =
                    customer?.Email ?? "Unknown Email";

                var adminBody = $@"
<html>
<body style=""font-family:Arial,sans-serif;"">

    <h2>New Custom Tailoring Request</h2>

    <p>
        A new custom tailoring request has been submitted
        on <strong>Drey's Fashion</strong>.
    </p>

    <hr />

    <p>
        <strong>Request Number:</strong><br />
        #{request.Id}
    </p>

    <p>
        <strong>Customer:</strong><br />
        {customerName}
    </p>

    <p>
        <strong>Email:</strong><br />
        {customerEmail}
    </p>

    <p>
        <strong>Outfit Type:</strong><br />
        {request.OutfitType}
    </p>

    <p>
        <strong>Style Description:</strong><br />
        {request.StyleDescription}
    </p>

    <p>
        <strong>Fabric Preference:</strong><br />
        {request.FabricPreference ?? "Not specified"}
    </p>

    <p>
        <strong>Additional Instructions:</strong><br />
        {request.AdditionalInstructions ?? "None"}
    </p>

    <hr />

    <p>
        The request is currently
        <strong>Pending</strong>.
    </p>

    <p>
        Please review the request from the
        Drey's Fashion admin dashboard.
    </p>

</body>
</html>";

                await _emailService.SendAsync(
                    AdminEmail,
                    $"New Tailoring Request - #{request.Id}",
                    adminBody,
                    true);
            }
            catch
            {
                // Admin email failure must not prevent
                // the request from being created.
            }


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
        /// Updates the status of a tailoring request and
        /// notifies the customer.
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
                .Include(item => item.User)
                .FirstOrDefaultAsync(item =>
                    item.Id == requestId);

            if (request == null)
            {
                return false;
            }

            var oldStatus = request.Status;

            request.Status = status.Trim();
            request.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();


            // --------------------------------------------------------
            // Notify customer.
            // --------------------------------------------------------

            if (!string.Equals(
                    oldStatus,
                    request.Status,
                    StringComparison.OrdinalIgnoreCase))
            {
                await SendStatusUpdateEmailAsync(request);
            }

            return true;
        }


        // ============================================================
        // UPDATE QUOTE
        // ============================================================

        /// <summary>
        /// Updates the quoted amount and administrator notes,
        /// then notifies the customer.
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
                .Include(item => item.User)
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


            // --------------------------------------------------------
            // Notify customer about the quote.
            // --------------------------------------------------------

            await SendQuoteEmailAsync(request);

            return true;
        }


        // ============================================================
        // SEND STATUS UPDATE EMAIL
        // ============================================================

        private async Task SendStatusUpdateEmailAsync(
            TailoringRequest request)
        {
            try
            {
                var customer = request.User;

                if (customer == null ||
                    string.IsNullOrWhiteSpace(customer.Email))
                {
                    return;
                }

                var customerName =
                    string.IsNullOrWhiteSpace(customer.FullName)
                        ? "Customer"
                        : customer.FullName;

                var customerBody = $@"
<html>
<body style=""font-family:Arial,sans-serif;"">

    <h2>Tailoring Request Update</h2>

    <p>
        Hello <strong>{customerName}</strong>,
    </p>

    <p>
        There has been an update to your
        <strong>Drey's Fashion</strong> tailoring request.
    </p>

    <hr />

    <p>
        <strong>Request Number:</strong><br />
        #{request.Id}
    </p>

    <p>
        <strong>Outfit Type:</strong><br />
        {request.OutfitType}
    </p>

    <p>
        <strong>New Status:</strong><br />
        {request.Status}
    </p>

    <hr />

    <p>
        Please check your Drey's Fashion account
        for more information.
    </p>

    <p>
        <strong>Drey's Fashion</strong>
    </p>

</body>
</html>";

                await _emailService.SendAsync(
                    customer.Email,
                    $"Tailoring Request #{request.Id} Update",
                    customerBody,
                    true);
            }
            catch
            {
                // Email failure must not prevent
                // the status update.
            }
        }


        // ============================================================
        // SEND QUOTE EMAIL
        // ============================================================

        private async Task SendQuoteEmailAsync(
            TailoringRequest request)
        {
            try
            {
                var customer = request.User;

                if (customer == null ||
                    string.IsNullOrWhiteSpace(customer.Email))
                {
                    return;
                }

                var customerName =
                    string.IsNullOrWhiteSpace(customer.FullName)
                        ? "Customer"
                        : customer.FullName;

                var customerBody = $@"
<html>
<body style=""font-family:Arial,sans-serif;"">

    <h2>Your Tailoring Quote Is Ready</h2>

    <p>
        Hello <strong>{customerName}</strong>,
    </p>

    <p>
        We have reviewed your custom tailoring request
        and prepared a quote for you.
    </p>

    <hr />

    <p>
        <strong>Request Number:</strong><br />
        #{request.Id}
    </p>

    <p>
        <strong>Outfit Type:</strong><br />
        {request.OutfitType}
    </p>

    <p>
        <strong>Quoted Amount:</strong><br />
        ₦{request.QuotedAmount:N0}
    </p>

    <p>
        <strong>Admin Notes:</strong><br />
        {request.AdminNotes ?? "No additional notes."}
    </p>

    <p>
        <strong>Status:</strong><br />
        Quote Provided
    </p>

    <hr />

    <p>
        Please sign in to your Drey's Fashion account
        to review the request and quote.
    </p>

    <p>
        <strong>Drey's Fashion</strong>
    </p>

</body>
</html>";

                await _emailService.SendAsync(
                    customer.Email,
                    $"Tailoring Quote - Request #{request.Id}",
                    customerBody,
                    true);
            }
            catch
            {
                // Email failure must not prevent
                // the quote from being saved.
            }
        }
    }
}