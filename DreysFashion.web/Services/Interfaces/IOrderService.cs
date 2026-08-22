using DreysFashion.web.Models;
using DreysFashion.web.ViewModels;

namespace DreysFashion.web.Services.Interfaces
{
    /// <summary>
    /// Defines operations for creating and managing customer orders.
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Creates a new order for an authenticated customer using
        /// the customer's checkout information and shopping cart.
        /// </summary>
        /// <param name="userId">
        /// The Identity ID of the authenticated customer.
        /// </param>
        /// <param name="checkout">
        /// The customer's checkout information.
        /// </param>
        /// <param name="cartItems">
        /// The items currently in the shopping cart.
        /// </param>
        /// <returns>
        /// The identifier of the newly created order.
        /// </returns>
        Task<int> CreateOrderAsync(
            string userId,
            CheckoutViewModel checkout,
            IReadOnlyList<CartItemViewModel> cartItems);

        /// <summary>
        /// Retrieves an order and its associated items and products.
        /// </summary>
        /// <param name="orderId">
        /// The unique identifier of the order.
        /// </param>
        /// <returns>
        /// The order if it exists; otherwise, null.
        /// </returns>
        Task<Order?> GetOrderByIdAsync(int orderId);

        /// <summary>
        /// Retrieves an order belonging to a specific authenticated user.
        /// </summary>
        /// <param name="orderId">
        /// The unique identifier of the order.
        /// </param>
        /// <param name="userId">
        /// The Identity ID of the authenticated customer.
        /// </param>
        /// <returns>
        /// The order if it belongs to the specified user;
        /// otherwise, null.
        /// </returns>
        Task<Order?> GetOrderByIdForUserAsync(
            int orderId,
            string userId);

        /// <summary>
        /// Associates a Paystack payment reference with an order.
        /// </summary>
        /// <param name="orderId">
        /// The unique identifier of the order.
        /// </param>
        /// <param name="paymentReference">
        /// The Paystack transaction reference.
        /// </param>
        Task SetPaymentReferenceAsync(
            int orderId,
            string paymentReference);

        /// <summary>
        /// Retrieves an order using its Paystack payment reference.
        /// </summary>
        /// <param name="paymentReference">
        /// The Paystack transaction reference.
        /// </param>
        /// <returns>
        /// The matching order if found; otherwise, null.
        /// </returns>
        Task<Order?> GetOrderByPaymentReferenceAsync(
            string paymentReference);

        /// <summary>
        /// Marks an order as paid after successful payment verification.
        /// </summary>
        /// <param name="orderId">
        /// The unique identifier of the order.
        /// </param>
        Task MarkOrderAsPaidAsync(int orderId);

        /// <summary>
        /// Retrieves all orders belonging to a specific authenticated user.
        /// </summary>
        /// <param name="userId">
        /// The Identity ID of the authenticated customer.
        /// </param>
        /// <returns>
        /// A list of the customer's orders, newest first.
        /// </returns>
        Task<List<Order>> GetOrdersByUserIdAsync(
            string userId);

        /// <summary>
        /// Retrieves all orders associated with a customer's email address.
        /// This method is retained for compatibility with existing orders
        /// and legacy functionality.
        /// </summary>
        /// <param name="email">
        /// The customer's email address.
        /// </param>
        /// <returns>
        /// A collection of the customer's orders.
        /// </returns>
        Task<List<Order>> GetOrdersByCustomerEmailAsync(
            string email);
    }
}