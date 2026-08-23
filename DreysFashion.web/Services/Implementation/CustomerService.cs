using DreysFashion.web.Data;
using DreysFashion.web.Models;
using DreysFashion.web.Services.Interfaces;
using DreysFashion.web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DreysFashion.web.Services
{
    /// <summary>
    /// Provides business logic for working with customer accounts.
    /// </summary>
    public class CustomerService : ICustomerService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="CustomerService"/> class.
        /// </summary>
        public CustomerService(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        /// <summary>
        /// Retrieves all registered customers with order statistics.
        /// </summary>
        public async Task<List<CustomerViewModel>> GetAllCustomersAsync()
        {
            return await _userManager.Users
                .AsNoTracking()
                .Select(user => new CustomerViewModel
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email ?? string.Empty,
                    PhoneNumber = user.PhoneNumber ?? string.Empty,
                    CreatedAt = user.CreatedAt,

                    OrderCount = _context.Orders
                        .Count(order => order.UserId == user.Id),

                    TotalSpent = _context.Orders
                        .Where(order =>
                            order.UserId == user.Id &&
                            order.Status != "Cancelled")
                        .Sum(order => (decimal?)order.TotalAmount) ?? 0
                })
                .OrderByDescending(customer => customer.CreatedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a customer and their order statistics by ID.
        /// </summary>
        public async Task<CustomerViewModel?> GetCustomerByIdAsync(
            string id)
        {
            return await _userManager.Users
                .AsNoTracking()
                .Where(user => user.Id == id)
                .Select(user => new CustomerViewModel
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email ?? string.Empty,
                    PhoneNumber = user.PhoneNumber ?? string.Empty,
                    CreatedAt = user.CreatedAt,

                    OrderCount = _context.Orders
                        .Count(order => order.UserId == user.Id),

                    TotalSpent = _context.Orders
                        .Where(order =>
                            order.UserId == user.Id &&
                            order.Status != "Cancelled")
                        .Sum(order => (decimal?)order.TotalAmount) ?? 0
                })
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Retrieves all orders belonging to a customer.
        /// </summary>
        public async Task<List<Order>> GetCustomerOrdersAsync(
            string customerId)
        {
            return await _context.Orders
                .AsNoTracking()
                .Where(order => order.UserId == customerId)
                .Include(order => order.OrderItems)
                .OrderByDescending(order => order.CreatedAt)
                .ToListAsync();
        }
    }
}