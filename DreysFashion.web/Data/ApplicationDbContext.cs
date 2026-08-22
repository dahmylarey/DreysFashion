
using DreysFashion.web.Models;
using Microsoft.EntityFrameworkCore;

namespace DreysFashion.web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();
    }
}
