using DreysFashion.web.ViewModels;

namespace DreysFashion.web.Services.Interfaces
{
    /// <summary>
    /// Defines operations for managing the customer's shopping cart.
    /// </summary>
    public interface ICartService
    {
        /// <summary>
        /// Occurs when the contents of the shopping cart change.
        /// </summary>
        event Action? CartChanged;

        /// <summary>
        /// Gets all items currently in the shopping cart.
        /// </summary>
        /// <returns>A list of cart items.</returns>
        IReadOnlyList<CartItemViewModel> GetItems();

        /// <summary>
        /// Adds a product to the shopping cart.
        /// </summary>
        /// <param name="product">The product to add.</param>
        /// <param name="quantity">The quantity to add.</param>
        void AddToCart(ProductViewModel product, int quantity);

        /// <summary>
        /// Removes a product completely from the shopping cart.
        /// </summary>
        /// <param name="productId">The unique identifier of the product.</param>
        void RemoveFromCart(int productId);

        /// <summary>
        /// Updates the quantity of a product already in the shopping cart.
        /// </summary>
        /// <param name="productId">The unique identifier of the product.</param>
        /// <param name="quantity">The new quantity.</param>
        void UpdateQuantity(int productId, int quantity);

        /// <summary>
        /// Removes all products from the shopping cart.
        /// </summary>
        void ClearCart();

        /// <summary>
        /// Gets the total number of products in the cart.
        /// </summary>
        int GetItemCount();

        /// <summary>
        /// Gets the total price of all products in the cart.
        /// </summary>
        decimal GetTotal();
    }
}