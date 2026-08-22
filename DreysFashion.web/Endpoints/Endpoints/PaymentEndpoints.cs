using DreysFashion.web.Services.Interfaces;

namespace DreysFashion.web.Endpoints
{
    /// <summary>
    /// Provides HTTP endpoints for payment operations.
    /// </summary>
    public static class PaymentEndpoints
    {
        /// <summary>
        /// Maps payment-related HTTP endpoints.
        /// </summary>
        public static void MapPaymentEndpoints(
            this IEndpointRouteBuilder app)
        {
            app.MapGet(
                "/payment/callback",
                VerifyPaymentAsync);
        }

        /// <summary>
        /// Handles the Paystack callback after payment.
        /// </summary>
        private static async Task<IResult> VerifyPaymentAsync(
            string? reference,
            IPaymentService paymentService,
            IOrderService orderService)
        {
            if (string.IsNullOrWhiteSpace(reference))
            {
                return Results.BadRequest(
                    "Payment reference was not provided.");
            }

            try
            {
                // Verify the transaction directly with Paystack.
                var payment =
                    await paymentService.VerifyPaymentAsync(
                        reference);

                if (payment == null)
                {
                    return Results.BadRequest(
                        "Unable to verify the Paystack transaction.");
                }

                // Make sure Paystack confirms the payment as successful.
                if (!string.Equals(
                        payment.Status,
                        "success",
                        StringComparison.OrdinalIgnoreCase))
                {
                    return Results.BadRequest(
                        $"Payment was not successful. " +
                        $"Current status: {payment.Status}");
                }

                // Find our order using the Paystack reference.
                var order =
                    await orderService
                        .GetOrderByPaymentReferenceAsync(reference);

                if (order == null)
                {
                    return Results.NotFound(
                        "The order associated with this payment " +
                        "could not be found.");
                }

                // Prevent marking an already-paid order again.
                if (!string.Equals(
                        order.Status,
                        "Paid",
                        StringComparison.OrdinalIgnoreCase))
                {
                    await orderService.MarkOrderAsPaidAsync(order.Id);
                }

                // Send the customer to the confirmation page.
                return Results.Redirect(
                    $"/order-confirmation/{order.Id}");
            }
            catch (Exception)
            {
                return Results.BadRequest(
                    "We could not verify your payment.");
            }
        }
    }
}