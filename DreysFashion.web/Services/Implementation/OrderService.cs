using DreysFashion.web.Data;
using DreysFashion.web.Models;
using DreysFashion.web.Services.Interfaces;
using DreysFashion.web.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DreysFashion.web.Services
{
    /// <summary>
    /// Provides business logic for creating and managing customer orders.
    /// </summary>
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderService"/> class.
        /// </summary>
        /// <param name="context">
        /// The application's database context.
        /// </param>
        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new order from the customer's checkout information
        /// and the current shopping cart.
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
        public async Task<int> CreateOrderAsync(
            CheckoutViewModel checkout,
            IReadOnlyList<CartItemViewModel> cartItems)
        {
            if (cartItems == null || cartItems.Count == 0)
            {
                throw new InvalidOperationException(
                    "Cannot create an order with an empty cart.");
            }

            // Create the order.
            var order = new Order
            {
                CustomerName = checkout.CustomerName,
                CustomerEmail = checkout.CustomerEmail,
                CustomerPhone = checkout.CustomerPhone,
                DeliveryAddress = checkout.DeliveryAddress,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            // Calculate the order total.
            order.TotalAmount = cartItems.Sum(
                item => item.UnitPrice * item.Quantity);

            // Create order items.
            foreach (var cartItem in cartItems)
            {
                var productExists = await _context.Products
                    .AnyAsync(product => product.Id == cartItem.ProductId);

                if (!productExists)
                {
                    throw new InvalidOperationException(
                        $"Product with ID {cartItem.ProductId} no longer exists.");
                }

                var orderItem = new OrderItem
                {
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.UnitPrice
                };

                order.OrderItems.Add(orderItem);
            }

            // Add the complete order to the database.
            _context.Orders.Add(order);

            await _context.SaveChangesAsync();

            return order.Id;
        }

        /// <summary>
        /// Retrieves an order together with its order items and products.
        /// </summary>
        /// <param name="orderId">
        /// The unique identifier of the order.
        /// </param>
        /// <returns>
        /// The requested order if it exists; otherwise, null.
        /// </returns>
        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(order => order.OrderItems)
                .ThenInclude(item => item.Product)
                .FirstOrDefaultAsync(order => order.Id == orderId);
        }

        /// <summary>
        /// Associates a Paystack payment reference with an existing order.
        /// </summary>
        /// <param name="orderId">
        /// The unique identifier of the order.
        /// </param>
        /// <param name="paymentReference">
        /// The Paystack transaction reference.
        /// </param>
        /// <summary>
        /// Associates a Paystack payment reference with an existing order.
        /// </summary>
        /// <param name="orderId">
        /// The unique identifier of the order.
        /// </param>
        /// <param name="paymentReference">
        /// The Paystack transaction reference.
        /// </param>
        public async Task SetPaymentReferenceAsync(
            int orderId,
            string paymentReference)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(order => order.Id == orderId);

            if (order == null)
            {
                throw new InvalidOperationException(
                    $"Order with ID {orderId} was not found.");
            }

            order.PaymentReference = paymentReference;

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Marks an order as paid.
        /// </summary>
        /// <param name="orderId">
        /// The unique identifier of the order.
        /// </param>
        public async Task MarkOrderAsPaidAsync(int orderId)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(order => order.Id == orderId);

            if (order == null)
            {
                throw new InvalidOperationException(
                    $"Order with ID {orderId} was not found.");
            }

            order.Status = "Paid";

            await _context.SaveChangesAsync();
        }

        //// <summary>
        /// Retrieves an order using its Paystack payment reference.
        /// </summary>
        /// <param name="paymentReference">
        /// The Paystack transaction reference.
        /// </param>
        /// <returns>
        /// The matching order if found; otherwise, null.
        /// </returns>
        public async Task<Order?> GetOrderByPaymentReferenceAsync(
            string paymentReference)
        {
            return await _context.Orders
                .Include(order => order.OrderItems)
                .ThenInclude(item => item.Product)
                .FirstOrDefaultAsync(
                    order => order.PaymentReference == paymentReference);
        }
    }
}