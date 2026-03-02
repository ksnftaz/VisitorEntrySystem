using Microsoft.EntityFrameworkCore;
using VisitorEntrySystem.Models;

namespace VisitorEntrySystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<VisitorRecord> Visitors { get; set; }
        public DbSet<User> Users { get; set; }
    }
}