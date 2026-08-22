using DreysFashion.web.Models;
using DreysFashion.web.ViewModels;

namespace DreysFashion.web.Services.Interfaces
{
    /// <summary>
    /// Defines operations for creating and retrieving customer orders.
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Creates a new order from the customer's checkout information
        /// and the items currently in the shopping cart.
        /// </summary>
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


    }
}