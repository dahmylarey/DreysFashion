using DreysFashion.web.ViewModels;
using Microsoft.AspNetCore.Components.Forms;

namespace DreysFashion.web.Services.Interfaces
{
    /// <summary>
    /// Defines operations for working with products.
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Retrieves all products.
        /// </summary>
        Task<List<ProductViewModel>> GetAllProductsAsync();

        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        Task<ProductViewModel?> GetProductByIdAsync(int id);

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="product">
        /// Product information.
        /// </param>
        /// <param name="imageFile">
        /// Optional product image.
        /// </param>
        Task<int> CreateProductAsync(
            ProductViewModel product,
            IBrowserFile? imageFile = null);

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="product">
        /// Updated product information.
        /// </param>
        /// <param name="imageFile">
        /// Optional new product image.
        /// </param>
        Task<bool> UpdateProductAsync(
            ProductViewModel product,
            IBrowserFile? imageFile = null);

        /// <summary>
        /// Deletes a product.
        /// </summary>
        Task<bool> DeleteProductAsync(int id);
    }
}