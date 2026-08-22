using DreysFashion.web.Data;
using DreysFashion.web.Services.Interfaces;
using DreysFashion.web.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DreysFashion.web.Services
{
    /// <summary>
    /// Provides business logic for working with products.
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductService"/> class.
        /// </summary>
        /// <param name="context">
        /// The database context used to access product data.
        /// </param>
        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all products available in the store.
        /// </summary>
        /// <returns>A list of product view models.</returns>
        public async Task<List<ProductViewModel>> GetAllProductsAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .Select(product => new ProductViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    StockQuantity = product.StockQuantity,
                    ImageUrl = product.ImageUrl,
                    IsAvailable = product.IsAvailable,
                })
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <returns>
        /// The product view model if found; otherwise, null.
        /// </returns>
        public async Task<ProductViewModel?> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(product => product.Id == id)
                .Select(product => new ProductViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    StockQuantity = product.StockQuantity,
                    ImageUrl = product.ImageUrl,
                    IsAvailable = product.IsAvailable,
                })
                .FirstOrDefaultAsync();
        }
    }
}