using Microsoft.EntityFrameworkCore;

namespace AutoCrmApi.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Client> Clients { get; set; } = null!;
        public DbSet<Auto> Autos { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;

        public DbSet<Service> Services { get; set; } = null!;
        public DbSet<OrderService> OrderServices { get; set; } = null!;

        public DbSet<Part> Parts { get; set; } = null!;
        public DbSet<OrderPart> OrderParts { get; set; } = null!;

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {
            Database.EnsureCreated();
        }
    }
}
