using DreysFashion.web.Models;

namespace DreysFashion.web.Services.Interfaces
{
    /// <summary>
    /// Defines operations for creating and managing
    /// customer custom tailoring requests.
    /// </summary>
    public interface ITailoringService
    {
        /// <summary>
        /// Creates a new custom tailoring request for an authenticated customer.
        /// </summary>
        /// <param name="request">
        /// The tailoring request to create.
        /// </param>
        /// <returns>
        /// The newly created tailoring request.
        /// </returns>
        Task<TailoringRequest> CreateRequestAsync(
            TailoringRequest request);

        /// <summary>
        /// Retrieves a tailoring request by its identifier.
        /// </summary>
        /// <param name="requestId">
        /// The unique identifier of the tailoring request.
        /// </param>
        /// <returns>
        /// The tailoring request if found; otherwise, null.
        /// </returns>
        Task<TailoringRequest?> GetRequestByIdAsync(
            int requestId);

        /// <summary>
        /// Retrieves all tailoring requests belonging
        /// to a specific authenticated customer.
        /// </summary>
        /// <param name="userId">
        /// The Identity user ID.
        /// </param>
        /// <returns>
        /// The customer's tailoring requests.
        /// </returns>
        Task<List<TailoringRequest>> GetRequestsByUserIdAsync(
            string userId);

        /// <summary>
        /// Retrieves all tailoring requests for administrators.
        /// </summary>
        /// <returns>
        /// All tailoring requests, newest first.
        /// </returns>
        Task<List<TailoringRequest>> GetAllRequestsAsync();

        /// <summary>
        /// Updates the status of a tailoring request.
        /// </summary>
        /// <param name="requestId">
        /// The tailoring request identifier.
        /// </param>
        /// <param name="status">
        /// The new request status.
        /// </param>
        /// <returns>
        /// True if the request was updated; otherwise, false.
        /// </returns>
        Task<bool> UpdateStatusAsync(
            int requestId,
            string status);

        /// <summary>
        /// Adds or updates the administrator's quote and notes
        /// for a tailoring request.
        /// </summary>
        /// <param name="requestId">
        /// The tailoring request identifier.
        /// </param>
        /// <param name="quotedAmount">
        /// The amount quoted by the tailor.
        /// </param>
        /// <param name="adminNotes">
        /// Additional notes from the administrator.
        /// </param>
        /// <returns>
        /// True if the request was updated; otherwise, false.
        /// </returns>
        Task<bool> UpdateQuoteAsync(
            int requestId,
            decimal quotedAmount,
            string? adminNotes);
    }
}