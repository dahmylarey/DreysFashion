using DreysFashion.web.ViewModels;

namespace DreysFashion.web.Services.Interfaces
{
    /// <summary>
    /// Defines operations for processing customer payments.
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        /// Initializes a Paystack transaction for an order.
        /// </summary>
        /// <param name="email">
        /// The customer's email address.
        /// </param>
        /// <param name="amount">
        /// The order amount in Naira.
        /// </param>
        /// <param name="reference">
        /// A unique reference for the transaction.
        /// </param>
        /// <returns>
        /// Paystack transaction initialization information.
        /// </returns>
        Task<PaystackTransactionData> InitializePaymentAsync(
            string email,
            decimal amount,
            string reference);

        /// <summary>
        /// Verifies a Paystack transaction using its reference.
        /// </summary>
        /// <param name="reference">
        /// The Paystack transaction reference.
        /// </param>
        /// <returns>
        /// The verified transaction information.
        /// </returns>
        Task<PaystackVerificationData?> VerifyPaymentAsync(
            string reference);
    }
}