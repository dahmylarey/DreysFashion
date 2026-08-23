using DreysFashion.web.Models;
using DreysFashion.web.ViewModels;

namespace DreysFashion.web.Services.Interfaces
{
    /// <summary>
    /// Defines operations for managing customer accounts.
    /// </summary>
    public interface ICustomerService
    {
        /// <summary>
        /// Retrieves all registered customers with their order statistics.
        /// </summary>
        Task<List<CustomerViewModel>> GetAllCustomersAsync();

        /// <summary>
        /// Retrieves a customer by their unique identifier.
        /// </summary>
        Task<CustomerViewModel?> GetCustomerByIdAsync(string id);

        Task<List<Order>> GetCustomerOrdersAsync(string customerId);
    }
}