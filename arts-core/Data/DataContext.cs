using arts_core.Models;
using Microsoft.EntityFrameworkCore;

namespace arts_core.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Cart> Carts    { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Models.Type> Types { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Variant> Variants { get; set; }
        public DbSet<VariantAttribute> VariantAttributes { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasOne(u => u.RoleType)
                .WithMany(rt => rt.Users)
                .HasForeignKey(u => u.RoleTypeId)
                .OnDelete(DeleteBehavior.NoAction);
        }

    }
}
