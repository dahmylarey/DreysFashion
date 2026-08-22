using DreysFashion.web.Services.Interfaces;
using DreysFashion.web.ViewModels;

namespace DreysFashion.web.Services
{
    /// <summary>
    /// Provides functionality for managing the customer's shopping cart.
    /// </summary>
    public class CartService : ICartService
    {
        private readonly List<CartItemViewModel> _items = new();

        /// <summary>
        /// Occurs when the contents of the shopping cart change.
        /// </summary>
        public event Action? CartChanged;

        /// <summary>
        /// Gets all items currently in the shopping cart.
        /// </summary>
        /// <returns>A read-only list of cart items.</returns>
        public IReadOnlyList<CartItemViewModel> GetItems()
        {
            return _items.AsReadOnly();
        }

        /// <summary>
        /// Adds a product to the shopping cart.
        /// If the product already exists, its quantity is increased.
        /// </summary>
        /// <param name="product">The product to add.</param>
        /// <param name="quantity">The quantity to add.</param>
        public void AddToCart(ProductViewModel product, int quantity)
        {
            if (quantity <= 0)
            {
                return;
            }

            var existingItem = _items
                .FirstOrDefault(item => item.ProductId == product.Id);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                _items.Add(new CartItemViewModel
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ImageUrl = product.ImageUrl,
                    UnitPrice = product.Price,
                    Quantity = quantity
                });
            }

            CartChanged?.Invoke();
        }

        /// <summary>
        /// Removes a product completely from the shopping cart.
        /// </summary>
        /// <param name="productId">The unique identifier of the product.</param>
        public void RemoveFromCart(int productId)
        {
            var item = _items
                .FirstOrDefault(item => item.ProductId == productId);

            if (item != null)
            {
                _items.Remove(item);

                CartChanged?.Invoke();
            }
        }

        /// <summary>
        /// Updates the quantity of a product already in the shopping cart.
        /// </summary>
        /// <param name="productId">The unique identifier of the product.</param>
        /// <param name="quantity">The new quantity.</param>
        public void UpdateQuantity(int productId, int quantity)
        {
            var item = _items
                .FirstOrDefault(item => item.ProductId == productId);

            if (item == null)
            {
                return;
            }

            if (quantity <= 0)
            {
                _items.Remove(item);
            }
            else
            {
                item.Quantity = quantity;
            }

            CartChanged?.Invoke();
        }

        /// <summary>
        /// Removes all products from the shopping cart.
        /// </summary>
        public void ClearCart()
        {
            _items.Clear();

            CartChanged?.Invoke();
        }

        /// <summary>
        /// Gets the total number of products in the shopping cart.
        /// </summary>
        /// <returns>The total quantity of products.</returns>
        public int GetItemCount()
        {
            return _items.Sum(item => item.Quantity);
        }

        /// <summary>
        /// Gets the total price of all products in the shopping cart.
        /// </summary>
        /// <returns>The total cart value.</returns>
        public decimal GetTotal()
        {
            return _items.Sum(item => item.TotalPrice);
        }
    }
}