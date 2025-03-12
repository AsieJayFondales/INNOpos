using InnoSpend.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InnoSpend.Services
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<CustomerInfo> Customers { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseItem> PurchaseItems { get; set; } //v3.0.2
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Purchase entity v3.0.3 incognitoversion 1 
            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.Property(e => e.Amount).HasPrecision(18, 2);
                entity.Property(e => e.Discount).HasPrecision(18, 2);
                entity.Property(e => e.NetAmount).HasPrecision(18, 2);
            });

            // Configure PurchaseItem entity v3.0.3 incognitoversion 1
            modelBuilder.Entity<PurchaseItem>(entity =>
            {
                entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
                entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
                entity.Property(e => e.Discount).HasPrecision(18, 2);
            });
        }

        public DbSet<Sale> Sales { get; set; }
    }


}
