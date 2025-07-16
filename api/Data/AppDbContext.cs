using Microsoft.EntityFrameworkCore;
using APIPractice.Models;

namespace APIPractice.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }
    }
}
