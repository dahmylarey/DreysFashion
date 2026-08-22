using DreysFashion.web.ViewModels;

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
        Task<int> CreateProductAsync(ProductViewModel product);

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        Task<bool> UpdateProductAsync(ProductViewModel product);

        /// <summary>
        /// Deletes a product.
        /// </summary>
        Task<bool> DeleteProductAsync(int id);
    }
}