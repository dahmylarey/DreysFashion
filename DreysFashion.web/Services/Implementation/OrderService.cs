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
        /// Initializes a new instance of the
        /// <see cref="OrderService"/> class.
        /// </summary>
        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }


        // ============================================================
        // CREATE ORDER
        // ============================================================

        /// <summary>
        /// Creates a new order for the authenticated customer
        /// using the customer's checkout information and cart items.
        /// </summary>
        public async Task<int> CreateOrderAsync(
            string userId,
            CheckoutViewModel checkout,
            IReadOnlyList<CartItemViewModel> cartItems)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new InvalidOperationException(
                    "A valid authenticated user is required to create an order.");
            }

            if (cartItems == null || cartItems.Count == 0)
            {
                throw new InvalidOperationException(
                    "Cannot create an order with an empty cart.");
            }

            // --------------------------------------------------------
            // Validate every cart item before creating the order.
            // --------------------------------------------------------

            foreach (var cartItem in cartItems)
            {
                if (cartItem.Quantity <= 0)
                {
                    throw new InvalidOperationException(
                        $"Invalid quantity for product {cartItem.ProductId}.");
                }

                var product = await _context.Products
                    .FirstOrDefaultAsync(product =>
                        product.Id == cartItem.ProductId);

                if (product == null)
                {
                    throw new InvalidOperationException(
                        $"Product with ID {cartItem.ProductId} no longer exists.");
                }

                if (!product.IsAvailable)
                {
                    throw new InvalidOperationException(
                        $"'{product.Name}' is currently unavailable.");
                }

                if (product.StockQuantity < cartItem.Quantity)
                {
                    throw new InvalidOperationException(
                        $"Not enough stock for '{product.Name}'. " +
                        $"Only {product.StockQuantity} unit(s) available.");
                }
            }

            // --------------------------------------------------------
            // Create the order.
            // --------------------------------------------------------

            var order = new Order
            {
                UserId = userId,

                CustomerName = checkout.CustomerName,
                CustomerEmail = checkout.CustomerEmail,
                CustomerPhone = checkout.CustomerPhone,
                DeliveryAddress = checkout.DeliveryAddress,

                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            // --------------------------------------------------------
            // Calculate the order total using the CURRENT
            // product prices from the database.
            // --------------------------------------------------------

            foreach (var cartItem in cartItems)
            {
                var product = await _context.Products
                    .FirstAsync(product =>
                        product.Id == cartItem.ProductId);

                order.TotalAmount +=
                    product.Price * cartItem.Quantity;

                var orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = cartItem.Quantity,

                    // Store the actual database price
                    // at the time of purchase.
                    UnitPrice = product.Price
                };

                order.OrderItems.Add(orderItem);
            }

            // --------------------------------------------------------
            // Add the complete order to the database.
            // --------------------------------------------------------

            _context.Orders.Add(order);

            await _context.SaveChangesAsync();

            return order.Id;
        }


        // ============================================================
        // GET ORDER BY ID
        // ============================================================

        /// <summary>
        /// Retrieves an order together with its products.
        /// </summary>
        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(order => order.OrderItems)
                .ThenInclude(item => item.Product)
                .FirstOrDefaultAsync(order =>
                    order.Id == orderId);
        }


        // ============================================================
        // GET ORDER BY ID FOR USER
        // ============================================================

        /// <summary>
        /// Retrieves an order only when it belongs to
        /// the specified authenticated customer.
        /// </summary>
        public async Task<Order?> GetOrderByIdForUserAsync(
            int orderId,
            string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return null;
            }

            return await _context.Orders
                .Include(order => order.OrderItems)
                .ThenInclude(item => item.Product)
                .FirstOrDefaultAsync(order =>
                    order.Id == orderId &&
                    order.UserId == userId);
        }


        // ============================================================
        // SET PAYMENT REFERENCE
        // ============================================================

        /// <summary>
        /// Associates a Paystack payment reference with an order.
        /// </summary>
        public async Task SetPaymentReferenceAsync(
            int orderId,
            string paymentReference)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(order =>
                    order.Id == orderId);

            if (order == null)
            {
                throw new InvalidOperationException(
                    $"Order with ID {orderId} was not found.");
            }

            order.PaymentReference = paymentReference;

            await _context.SaveChangesAsync();
        }


        // ============================================================
        // MARK ORDER AS PAID + DEDUCT STOCK
        // ============================================================

        /// <summary>
        /// Marks an order as paid and deducts the purchased quantities
        /// from product inventory.
        ///
        /// If the order has already been paid, no changes are made.
        /// This prevents stock from being deducted more than once.
        /// </summary>
        public async Task MarkOrderAsPaidAsync(int orderId)
        {
            // --------------------------------------------------------
            // Load the order together with its order items
            // and their associated products.
            // --------------------------------------------------------

            var order = await _context.Orders
                .Include(order => order.OrderItems)
                .ThenInclude(item => item.Product)
                .FirstOrDefaultAsync(order =>
                    order.Id == orderId);

            if (order == null)
            {
                throw new InvalidOperationException(
                    $"Order with ID {orderId} was not found.");
            }

            // --------------------------------------------------------
            // IMPORTANT:
            // If the order is already paid, do nothing.
            //
            // This prevents stock from being deducted twice if
            // Paystack verification happens more than once.
            // --------------------------------------------------------

            if (string.Equals(
                    order.Status,
                    "Paid",
                    StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            // --------------------------------------------------------
            // Validate ALL stock before changing ANY stock.
            // --------------------------------------------------------

            foreach (var orderItem in order.OrderItems)
            {
                var product = orderItem.Product;

                if (product == null)
                {
                    throw new InvalidOperationException(
                        $"Product for order item {orderItem.Id} could not be found.");
                }

                if (!product.IsAvailable)
                {
                    throw new InvalidOperationException(
                        $"'{product.Name}' is no longer available.");
                }

                if (product.StockQuantity < orderItem.Quantity)
                {
                    throw new InvalidOperationException(
                        $"Not enough stock for '{product.Name}'. " +
                        $"Only {product.StockQuantity} unit(s) remain.");
                }
            }

            // --------------------------------------------------------
            // All stock is available.
            // Deduct the purchased quantities.
            // --------------------------------------------------------

            foreach (var orderItem in order.OrderItems)
            {
                var product = orderItem.Product;

                product.StockQuantity -= orderItem.Quantity;

                // Automatically disable the product when
                // no units remain.
                if (product.StockQuantity <= 0)
                {
                    product.StockQuantity = 1;
                    product.IsAvailable = false;
                }
            }

            // --------------------------------------------------------
            // Mark the order as paid.
            // --------------------------------------------------------

            order.Status = "Paid";

            // --------------------------------------------------------
            // Save BOTH the stock changes and the order status.
            // --------------------------------------------------------

            await _context.SaveChangesAsync();
        }


        // ============================================================
        // GET ORDER BY PAYMENT REFERENCE
        // ============================================================

        /// <summary>
        /// Retrieves an order using its Paystack payment reference.
        /// </summary>
        public async Task<Order?> GetOrderByPaymentReferenceAsync(
            string paymentReference)
        {
            return await _context.Orders
                .Include(order => order.OrderItems)
                .ThenInclude(item => item.Product)
                .FirstOrDefaultAsync(order =>
                    order.PaymentReference == paymentReference);
        }


        // ============================================================
        // GET ORDERS BY CUSTOMER EMAIL
        // ============================================================

        /// <summary>
        /// Retrieves all orders associated with a customer's email.
        /// </summary>
        public async Task<List<Order>> GetOrdersByCustomerEmailAsync(
            string email)
        {
            return await _context.Orders
                .Where(order =>
                    order.CustomerEmail == email)
                .OrderByDescending(order =>
                    order.CreatedAt)
                .ToListAsync();
        }


        // ============================================================
        // GET ORDERS BY USER ID
        // ============================================================

        /// <summary>
        /// Retrieves all orders belonging to a specific authenticated user.
        /// </summary>
        public async Task<List<Order>> GetOrdersByUserIdAsync(
            string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return new List<Order>();
            }

            return await _context.Orders
                .Where(order =>
                    order.UserId == userId)
                .OrderByDescending(order =>
                    order.CreatedAt)
                .ToListAsync();
        }


        // ============================================================
        // GET ALL ORDERS
        // ============================================================

        /// <summary>
        /// Retrieves all orders in the system.
        /// </summary>
        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(order => order.OrderItems)
                .ThenInclude(item => item.Product)
                .OrderByDescending(order =>
                    order.CreatedAt)
                .ToListAsync();
        }


        // ============================================================
        // UPDATE ORDER STATUS
        // ============================================================

        /// <summary>
        /// Updates the status of an existing order.
        /// </summary>
        public async Task<bool> UpdateOrderStatusAsync(
            int orderId,
            string status)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(order =>
                    order.Id == orderId);

            if (order == null)
            {
                return false;
            }

            order.Status = status;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}