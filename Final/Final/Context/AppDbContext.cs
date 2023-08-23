using Microsoft.EntityFrameworkCore;
using Final.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Final
{
    public class AppDbContext : IdentityDbContext
    {
        private readonly DbContextOptions _options;

        public AppDbContext(DbContextOptions options) : base(options)
        {
            _options = options;
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Types> Types { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<ProductTypes> ProductTypes { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
