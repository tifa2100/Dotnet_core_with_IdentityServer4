using Microsoft.EntityFrameworkCore;
using Project.API.Core.Models;

namespace Project.API.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {
        }
    }
}
