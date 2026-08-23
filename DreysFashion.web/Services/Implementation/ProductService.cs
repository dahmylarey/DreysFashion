using DreysFashion.web.Data;
using DreysFashion.web.Services.Interfaces;
using DreysFashion.web.ViewModels;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace DreysFashion.web.Services
{
    /// <summary>
    /// Provides business logic for working with products.
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        private const long MaxImageSize = 5 * 1024 * 1024;

        private static readonly string[] AllowedExtensions =
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".webp"
        };

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ProductService"/> class.
        /// </summary>
        public ProductService(
            ApplicationDbContext context,
            IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        /// <summary>
        /// Retrieves all products.
        /// </summary>
        public async Task<List<ProductViewModel>> GetAllProductsAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .OrderByDescending(product => product.CreatedAt)
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
            ProductViewModel product,
            IBrowserFile? imageFile = null)
        {
            string? imageUrl = null;

            if (imageFile != null)
            {
                imageUrl = await SaveImageAsync(imageFile);
            }

            var newProduct = new Models.Product
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                ImageUrl = imageUrl,
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
            ProductViewModel product,
            IBrowserFile? imageFile = null)
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
            existingProduct.IsAvailable = product.IsAvailable;

            if (imageFile != null)
            {
                var oldImageUrl = existingProduct.ImageUrl;

                var newImageUrl =
                    await SaveImageAsync(imageFile);

                existingProduct.ImageUrl = newImageUrl;

                DeleteImage(oldImageUrl);
            }

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

            var imageUrl = product.ImageUrl;

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            DeleteImage(imageUrl);

            return true;
        }

        /// <summary>
        /// Saves an uploaded product image to wwwroot/images/products.
        /// </summary>
        private async Task<string> SaveImageAsync(
            IBrowserFile imageFile)
        {
            if (imageFile.Size > MaxImageSize)
            {
                throw new InvalidOperationException(
                    "Product image cannot be larger than 5 MB.");
            }

            var extension =
                Path.GetExtension(imageFile.Name)
                    .ToLowerInvariant();

            if (!AllowedExtensions.Contains(extension))
            {
                throw new InvalidOperationException(
                    "Only JPG, JPEG, PNG and WEBP images are allowed.");
            }

            var uploadsFolder =
                Path.Combine(
                    _environment.WebRootPath,
                    "images",
                    "products");

            Directory.CreateDirectory(uploadsFolder);

            var fileName =
                $"{Guid.NewGuid():N}{extension}";

            var filePath =
                Path.Combine(
                    uploadsFolder,
                    fileName);

            await using var stream =
                new FileStream(
                    filePath,
                    FileMode.Create);

            await imageFile
                .OpenReadStream(MaxImageSize)
                .CopyToAsync(stream);

            return $"/images/products/{fileName}";
        }

        /// <summary>
        /// Deletes a previously uploaded product image.
        /// </summary>
        private void DeleteImage(string? imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return;
            }

            if (!imageUrl.StartsWith(
                    "/images/products/",
                    StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var fileName =
                Path.GetFileName(imageUrl);

            if (string.IsNullOrWhiteSpace(fileName))
            {
                return;
            }

            var filePath =
                Path.Combine(
                    _environment.WebRootPath,
                    "images",
                    "products",
                    fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}