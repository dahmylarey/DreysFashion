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
        private readonly IEmailService _emailService;

        // Change this to your actual admin email.
        private const string AdminEmail =
            "oladeleoluwada@gmail.com";

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="OrderService"/> class.
        /// </summary>
        public OrderService(
            ApplicationDbContext context,
            IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }


        // ============================================================
        // CREATE ORDER
        // ============================================================

        /// <summary>
        /// Creates a new order for the authenticated customer.
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
            // Validate every cart item.
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
            // Create order.
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
            // Calculate total using current database prices.
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
                    UnitPrice = product.Price
                };

                order.OrderItems.Add(orderItem);
            }

            _context.Orders.Add(order);

            await _context.SaveChangesAsync();

            return order.Id;
        }


        // ============================================================
        // GET ORDER BY ID
        // ============================================================

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
                    $"Order {orderId} was not found.");
            }

            order.PaymentReference = paymentReference;

            await _context.SaveChangesAsync();
        }


        // ============================================================
        // MARK ORDER AS PAID
        // ============================================================

        public async Task MarkOrderAsPaidAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(order => order.OrderItems)
                .ThenInclude(item => item.Product)
                .FirstOrDefaultAsync(order =>
                    order.Id == orderId);

            if (order == null)
            {
                throw new InvalidOperationException(
                    $"Order {orderId} was not found.");
            }

            // Prevent duplicate stock deduction.
            if (string.Equals(
                    order.Status,
                    "Paid",
                    StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            // --------------------------------------------------------
            // Validate all stock before changing anything.
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
            // Deduct stock.
            // --------------------------------------------------------

            foreach (var orderItem in order.OrderItems)
            {
                var product = orderItem.Product;

                product.StockQuantity -= orderItem.Quantity;

                // Correct: stock should be zero when sold out.
                if (product.StockQuantity <= 0)
                {
                    product.StockQuantity = 0;
                    product.IsAvailable = false;
                }
            }

            order.Status = "Paid";

            await _context.SaveChangesAsync();

            // --------------------------------------------------------
            // Payment confirmation emails.
            // --------------------------------------------------------

            await SendPaymentConfirmationEmailsAsync(order);
        }


        // ============================================================
        // PAYMENT CONFIRMATION EMAILS
        // ============================================================

        private async Task SendPaymentConfirmationEmailsAsync(
            Order order)
        {
            // Customer email
            try
            {
                var customerBody = $@"
<html>
<body style=""font-family:Arial,sans-serif;"">

    <h2>Payment Successful 🎉</h2>

    <p>
        Hello <strong>{order.CustomerName}</strong>,
    </p>

    <p>
        Your payment for order
        <strong>#{order.Id}</strong>
        has been successfully received.
    </p>

    <p>
        <strong>Amount Paid:</strong>
        ₦{order.TotalAmount:N0}
    </p>

    <p>
        <strong>Order Status:</strong>
        Payment Confirmed
    </p>

    <p>
        We have received your order and will begin processing it shortly.
    </p>

    <p>
        Thank you for shopping with
        <strong>Drey's Fashion</strong>.
    </p>

</body>
</html>";

                await _emailService.SendAsync(
                    order.CustomerEmail,
                    $"Payment Confirmed - Order #{order.Id}",
                    customerBody,
                    true);
            }
            catch
            {
                // Email failure must not break payment processing.
            }


            // Admin email
            try
            {
                var adminBody = $@"
<html>
<body style=""font-family:Arial,sans-serif;"">

    <h2>Payment Received</h2>

    <p>
        Payment has been successfully confirmed for
        <strong>Order #{order.Id}</strong>.
    </p>

    <hr />

    <p>
        <strong>Customer:</strong><br />
        {order.CustomerName}
    </p>

    <p>
        <strong>Email:</strong><br />
        {order.CustomerEmail}
    </p>

    <p>
        <strong>Phone:</strong><br />
        {order.CustomerPhone}
    </p>

    <p>
        <strong>Amount:</strong><br />
        ₦{order.TotalAmount:N0}
    </p>

    <p>
        <strong>Payment Reference:</strong><br />
        {order.PaymentReference}
    </p>

    <p>
        <strong>Delivery Address:</strong><br />
        {order.DeliveryAddress}
    </p>

    <hr />

    <p>
        The order is now ready for processing.
    </p>

</body>
</html>";

                await _emailService.SendAsync(
                    AdminEmail,
                    $"Payment Received - Order #{order.Id}",
                    adminBody,
                    true);
            }
            catch
            {
                // Email failure must not break payment processing.
            }
        }


        // ============================================================
        // GET ORDER BY PAYMENT REFERENCE
        // ============================================================

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

            // --------------------------------------------------------
            // Notify customer whenever admin changes order status.
            // --------------------------------------------------------

            try
            {
                var customerBody = $@"
<html>
<body style=""font-family:Arial,sans-serif;"">

    <h2>Order Update</h2>

    <p>
        Hello <strong>{order.CustomerName}</strong>,
    </p>

    <p>
        There has been an update to your
        <strong>Drey's Fashion</strong> order.
    </p>

    <p>
        <strong>Order Number:</strong>
        #{order.Id}
    </p>

    <p>
        <strong>New Status:</strong>
        {status}
    </p>

    <p>
        Thank you for shopping with
        <strong>Drey's Fashion</strong>.
    </p>

</body>
</html>";

                await _emailService.SendAsync(
                    order.CustomerEmail,
                    $"Order #{order.Id} Update - {status}",
                    customerBody,
                    true);
            }
            catch
            {
                // Email failure must not break status updates.
            }

            return true;
        }
    }
}