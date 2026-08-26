namespace DreysFashion.web.Services
{
    /// <summary>
    /// Provides branded email templates for Drey's Fashion.
    /// </summary>
    public class EmailTemplateService
    {
        private const string BrandName = "Drey's Fashion";

        /// <summary>
        /// Creates the common branded email layout.
        /// </summary>
        private string Layout(
            string title,
            string content)
        {
            return $"""
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset="UTF-8">
                    <meta name="viewport"
                          content="width=device-width, initial-scale=1.0">
                    <title>{title}</title>
                </head>

                <body style="
                    margin:0;
                    padding:0;
                    background:#f5f5f5;
                    font-family:Arial,Helvetica,sans-serif;
                    color:#222;
                ">

                    <div style="
                        max-width:600px;
                        margin:30px auto;
                        background:#ffffff;
                        border-radius:8px;
                        overflow:hidden;
                        box-shadow:0 2px 10px rgba(0,0,0,0.08);
                    ">

                        <!-- HEADER -->

                        <div style="
                            background:#111111;
                            color:#ffffff;
                            padding:28px;
                            text-align:center;
                        ">

                            <div style="
                                font-size:13px;
                                letter-spacing:3px;
                                margin-bottom:8px;
                            ">
                                DREY'S FASHION
                            </div>

                            <h1 style="
                                margin:0;
                                font-size:26px;
                                font-weight:600;
                            ">
                                {title}
                            </h1>

                        </div>

                        <!-- CONTENT -->

                        <div style="
                            padding:35px 30px;
                            line-height:1.7;
                        ">

                            {content}

                        </div>

                        <!-- FOOTER -->

                        <div style="
                            background:#f8f8f8;
                            padding:25px;
                            text-align:center;
                            color:#777;
                            font-size:13px;
                        ">

                            <strong style="color:#222;">
                                {BrandName}
                            </strong>

                            <p style="margin:8px 0 0;">
                                Wear Your Style. Made For You.
                            </p>

                            <p style="margin:8px 0 0;">
                                This is an automated email.
                                Please do not reply directly to this message.
                            </p>

                        </div>

                    </div>

                </body>
                </html>
                """;
        }


        /// <summary>
        /// Creates a welcome email for a newly registered customer.
        /// </summary>
        public string WelcomeCustomer(string customerName)
        {
            var content = $"""
                <p>
                    Hello <strong>{customerName}</strong>,
                </p>

                <p>
                    Welcome to <strong>{BrandName}</strong>.
                    We're happy to have you with us.
                </p>

                <p>
                    Your account has been successfully created.
                    You can now shop our collection, manage your orders,
                    save your measurements and request custom tailoring.
                </p>

                <div style="
                    text-align:center;
                    margin:30px 0;
                ">

                    <a href="/shop"
                       style="
                           display:inline-block;
                           background:#111111;
                           color:#ffffff;
                           padding:12px 24px;
                           text-decoration:none;
                           border-radius:5px;
                       ">
                        Shop Collection
                    </a>

                </div>

                <p>
                    Thank you for choosing Drey's Fashion.
                </p>

                <p>
                    <strong>
                        The Drey's Fashion Team
                    </strong>
                </p>
                """;

            return Layout(
                "Welcome to Drey's Fashion",
                content);
        }


        /// <summary>
        /// Creates an email notifying a customer that an order was created.
        /// </summary>
        public string OrderCreated(
            string customerName,
            int orderId,
            decimal totalAmount)
        {
            var content = $"""
                <p>
                    Hello <strong>{customerName}</strong>,
                </p>

                <p>
                    Thank you for shopping with Drey's Fashion.
                    We've received your order.
                </p>

                <div style="
                    background:#f8f8f8;
                    padding:20px;
                    border-radius:6px;
                    margin:25px 0;
                ">

                    <p style="margin:5px 0;">
                        <strong>Order Number:</strong>
                        #{orderId}
                    </p>

                    <p style="margin:5px 0;">
                        <strong>Total:</strong>
                        ₦{totalAmount:N0}
                    </p>

                    <p style="margin:5px 0;">
                        <strong>Status:</strong>
                        Pending Payment
                    </p>

                </div>

                <p>
                    Your order has been created successfully.
                    You will receive another email once your payment
                    has been confirmed.
                </p>

                <p>
                    <strong>
                        Drey's Fashion
                    </strong>
                </p>
                """;

            return Layout(
                $"Order #{orderId} Received",
                content);
        }


        /// <summary>
        /// Creates a payment confirmation email.
        /// </summary>
        public string PaymentSuccessful(
            string customerName,
            int orderId,
            decimal amount,
            string reference)
        {
            var content = $"""
                <p>
                    Hello <strong>{customerName}</strong>,
                </p>

                <p>
                    Your payment has been successfully received.
                    Thank you for your purchase.
                </p>

                <div style="
                    background:#f1f8f3;
                    border-left:4px solid #198754;
                    padding:20px;
                    border-radius:5px;
                    margin:25px 0;
                ">

                    <p style="margin:5px 0;">
                        <strong>Order:</strong>
                        #{orderId}
                    </p>

                    <p style="margin:5px 0;">
                        <strong>Amount Paid:</strong>
                        ₦{amount:N0}
                    </p>

                    <p style="margin:5px 0;">
                        <strong>Payment Reference:</strong>
                        {reference}
                    </p>

                    <p style="
                        margin:10px 0 0;
                        color:#198754;
                        font-weight:bold;
                    ">
                        Payment Successful
                    </p>

                </div>

                <p>
                    We'll begin processing your order and keep you
                    updated as it progresses.
                </p>

                <p>
                    <strong>
                        Drey's Fashion
                    </strong>
                </p>
                """;

            return Layout(
                $"Payment Confirmed - Order #{orderId}",
                content);
        }


        /// <summary>
        /// Creates an email notifying the customer of an order status change.
        /// </summary>
        public string OrderStatusChanged(
            string customerName,
            int orderId,
            string status)
        {
            var content = $"""
                <p>
                    Hello <strong>{customerName}</strong>,
                </p>

                <p>
                    There's an update regarding your Drey's Fashion order.
                </p>

                <div style="
                    background:#f8f8f8;
                    padding:20px;
                    border-radius:6px;
                    margin:25px 0;
                    text-align:center;
                ">

                    <p style="margin:5px 0;">
                        <strong>Order #{orderId}</strong>
                    </p>

                    <p style="
                        font-size:20px;
                        font-weight:bold;
                        margin:10px 0;
                    ">
                        {status}
                    </p>

                </div>

                <p>
                    Thank you for shopping with Drey's Fashion.
                </p>

                <p>
                    <strong>
                        The Drey's Fashion Team
                    </strong>
                </p>
                """;

            return Layout(
                $"Order #{orderId} Update",
                content);
        }


        /// <summary>
        /// Creates an email confirming a tailoring request.
        /// </summary>
        public string TailoringRequestCreated(
            string customerName,
            int requestId,
            string outfitType)
        {
            var content = $"""
                <p>
                    Hello <strong>{customerName}</strong>,
                </p>

                <p>
                    We've received your custom tailoring request.
                </p>

                <div style="
                    background:#f8f8f8;
                    padding:20px;
                    border-radius:6px;
                    margin:25px 0;
                ">

                    <p style="margin:5px 0;">
                        <strong>Request Number:</strong>
                        #{requestId}
                    </p>

                    <p style="margin:5px 0;">
                        <strong>Outfit:</strong>
                        {outfitType}
                    </p>

                    <p style="margin:5px 0;">
                        <strong>Status:</strong>
                        Pending Review
                    </p>

                </div>

                <p>
                    Our team will review your request and get back to you
                    with the next steps and quotation.
                </p>

                <p>
                    <strong>
                        Drey's Fashion
                    </strong>
                </p>
                """;

            return Layout(
                $"Tailoring Request #{requestId}",
                content);
        }


        /// <summary>
        /// Creates an email notifying a customer that a tailoring quote
        /// is available.
        /// </summary>
        public string TailoringQuote(
            string customerName,
            int requestId,
            string outfitType,
            decimal quote)
        {
            var content = $"""
                <p>
                    Hello <strong>{customerName}</strong>,
                </p>

                <p>
                    Your custom tailoring request has been reviewed.
                    We've prepared a quotation for you.
                </p>

                <div style="
                    background:#f8f8f8;
                    padding:20px;
                    border-radius:6px;
                    margin:25px 0;
                ">

                    <p style="margin:5px 0;">
                        <strong>Request:</strong>
                        #{requestId}
                    </p>

                    <p style="margin:5px 0;">
                        <strong>Outfit:</strong>
                        {outfitType}
                    </p>

                    <p style="
                        font-size:22px;
                        font-weight:bold;
                        margin:15px 0 5px;
                    ">
                        ₦{quote:N0}
                    </p>

                    <p style="margin:0;color:#777;">
                        Quoted Amount
                    </p>

                </div>

                <p>
                    Please log in to your Drey's Fashion account
                    to review your request and proceed.
                </p>

                <p>
                    <strong>
                        The Drey's Fashion Team
                    </strong>
                </p>
                """;

            return Layout(
                $"Tailoring Quote - Request #{requestId}",
                content);
        }


        /// <summary>
        /// Creates an email notifying a customer about a tailoring
        /// request status change.
        /// </summary>
        public string TailoringStatusChanged(
            string customerName,
            int requestId,
            string status)
        {
            var content = $"""
                <p>
                    Hello <strong>{customerName}</strong>,
                </p>

                <p>
                    Your custom tailoring request has been updated.
                </p>

                <div style="
                    background:#f8f8f8;
                    padding:20px;
                    border-radius:6px;
                    margin:25px 0;
                    text-align:center;
                ">

                    <p style="margin:5px 0;">
                        <strong>
                            Tailoring Request #{requestId}
                        </strong>
                    </p>

                    <p style="
                        font-size:20px;
                        font-weight:bold;
                        margin:10px 0;
                    ">
                        {status}
                    </p>

                </div>

                <p>
                    Please log in to your account for more details.
                </p>

                <p>
                    <strong>
                        Drey's Fashion
                    </strong>
                </p>
                """;

            return Layout(
                $"Tailoring Request #{requestId} Update",
                content);
        }


        /// <summary>
        /// Creates an administrator notification for a new customer.
        /// </summary>
        public string NewCustomerNotification(
            string customerName,
            string email)
        {
            var content = $"""
                <p>
                    A new customer has registered on Drey's Fashion.
                </p>

                <div style="
                    background:#f8f8f8;
                    padding:20px;
                    border-radius:6px;
                    margin:25px 0;
                ">

                    <p style="margin:5px 0;">
                        <strong>Name:</strong>
                        {customerName}
                    </p>

                    <p style="margin:5px 0;">
                        <strong>Email:</strong>
                        {email}
                    </p>

                </div>

                <p>
                    You can view the customer from the
                    Drey's Fashion admin dashboard.
                </p>
                """;

            return Layout(
                "New Customer Registration",
                content);
        }


        /// <summary>
        /// Creates an administrator notification for a new order.
        /// </summary>
        public string NewOrderAdminNotification(
            string customerName,
            string customerEmail,
            int orderId,
            decimal amount)
        {
            var content = $"""
                <p>
                    A new order has been placed on Drey's Fashion.
                </p>

                <div style="
                    background:#f8f8f8;
                    padding:20px;
                    border-radius:6px;
                    margin:25px 0;
                ">

                    <p style="margin:5px 0;">
                        <strong>Order:</strong>
                        #{orderId}
                    </p>

                    <p style="margin:5px 0;">
                        <strong>Customer:</strong>
                        {customerName}
                    </p>

                    <p style="margin:5px 0;">
                        <strong>Email:</strong>
                        {customerEmail}
                    </p>

                    <p style="margin:5px 0;">
                        <strong>Amount:</strong>
                        ₦{amount:N0}
                    </p>

                </div>

                <p>
                    Please log in to the admin dashboard
                    to review the order.
                </p>
                """;

            return Layout(
                $"New Order #{orderId}",
                content);
        }


        /// <summary>
        /// Creates an administrator notification for a new tailoring request.
        /// </summary>
        public string NewTailoringAdminNotification(
            string customerName,
            string customerEmail,
            int requestId,
            string outfitType)
        {
            var content = $"""
                <p>
                    A new custom tailoring request has been submitted.
                </p>

                <div style="
                    background:#f8f8f8;
                    padding:20px;
                    border-radius:6px;
                    margin:25px 0;
                ">

                    <p style="margin:5px 0;">
                        <strong>Request:</strong>
                        #{requestId}
                    </p>

                    <p style="margin:5px 0;">
                        <strong>Customer:</strong>
                        {customerName}
                    </p>

                    <p style="margin:5px 0;">
                        <strong>Email:</strong>
                        {customerEmail}
                    </p>

                    <p style="margin:5px 0;">
                        <strong>Outfit:</strong>
                        {outfitType}
                    </p>

                </div>

                <p>
                    Please review the request from the
                    admin dashboard.
                </p>
                """;

            return Layout(
                $"New Tailoring Request #{requestId}",
                content);
        }
    }
}