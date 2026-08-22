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
        /// Initializes a new instance of the
        /// <see cref="ProductService"/> class.
        /// </summary>
        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all products.
        /// </summary>
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
                    IsAvailable = product.IsAvailable
                })
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
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
                    IsAvailable = product.IsAvailable
                })
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        public async Task<int> CreateProductAsync(
            ProductViewModel product)
        {
            var newProduct = new Models.Product
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                ImageUrl = product.ImageUrl,
                IsAvailable = product.IsAvailable,
                CreatedAt = DateTime.UtcNow
            };

            _context.Products.Add(newProduct);

            await _context.SaveChangesAsync();

            return newProduct.Id;
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        public async Task<bool> UpdateProductAsync(
            ProductViewModel product)
        {
            var existingProduct =
                await _context.Products
                    .FirstOrDefaultAsync(
                        item => item.Id == product.Id);

            if (existingProduct == null)
            {
                return false;
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.StockQuantity = product.StockQuantity;
            existingProduct.ImageUrl = product.ImageUrl;
            existingProduct.IsAvailable = product.IsAvailable;

            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Deletes an existing product.
        /// </summary>
        public async Task<bool> DeleteProductAsync(int id)
        {
            var product =
                await _context.Products
                    .FirstOrDefaultAsync(
                        item => item.Id == id);

            if (product == null)
            {
                return false;
            }

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}