using DreysFashion.web.ViewModels;

namespace DreysFashion.web.Services.Interfaces
{
    /// <summary>
    /// Defines operations for retrieving and managing products.
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Retrieves all products available in the store.
        /// </summary>
        /// <returns>A list of product view models.</returns>
        Task<List<ProductViewModel>> GetAllProductsAsync();

        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <returns>The product view model if found; otherwise, null.</returns>
        Task<ProductViewModel?> GetProductByIdAsync(int id);
    }
}