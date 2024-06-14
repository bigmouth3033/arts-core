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
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<ProductEvent> ProductEvents { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Exchange> Exchanges { get; set; }
        public DbSet<Refund> Refunds { get; set; }
        public DbSet<Stock> Stocks {  get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasOne(u => u.RoleType)
                .WithMany(rt => rt.Users)
                .HasForeignKey(u => u.RoleTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.PaymentType)
                .WithMany(rt => rt.Payments)
                .HasForeignKey(p => p.PaymentTypeId)
                .OnDelete(DeleteBehavior.NoAction);           


            modelBuilder.Entity<Exchange>()
                .HasOne(e => e.OriginalOrder)
                .WithMany(o => o.Exchanges)
                .HasForeignKey(e => e.OriginalOrderId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Refund>()
                .HasOne(r => r.Payment)
                .WithMany(p => p.Refunds)
                .HasForeignKey(r => r.PaymentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Stock>().HasOne(s => s.Variant).WithMany(v => v.Stocks).HasForeignKey(s => s.VariantId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ProductImage>().HasOne(i => i.Product).WithMany(p => p.ProductImages).HasForeignKey(i => i.ProductId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Event>(option =>
            {
                option.Property(e => e.StartDate).HasDefaultValueSql("GETDATE()");
                option.Property(e => e.EndDate).HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<Exchange>(option =>
            {
                option.Property(e => e.ExchangeDate).HasDefaultValueSql("GETDATE()");        
            });


            modelBuilder.Entity<Message>(option =>
            {
                option.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<Order>(option =>
            {
                option.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<Product>(option =>
            {
                option.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<Stock>(option =>
            {
                option.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<Variant>(option =>
            {
                option.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<Models.Type>(option =>
            {
                option.HasData([
                    new Models.Type(){Id = 1,Name = "Size", NameType = "VariantAttribute"},
                    new Models.Type(){Id = 2, Name = "Color", NameType = "VariantAttribute"},
                    new Models.Type(){Id = 3, Name = "Material", NameType = "VariantAttribute"},
                    new Models.Type(){Id = 4, Name = "Admin", NameType = "UserRole"},
                    new Models.Type(){Id = 5, Name = "Employee", NameType = "UserRole"},
                    new Models.Type(){Id = 6, Name = "Customer", NameType = "UserRole"},
                    ]);
            });

            modelBuilder.Entity<Category>(option =>
            {
                option.HasData([
                    new Category(){Id = 1, Name="Arts"},
                    new Category(){Id = 2, Name="Gift Articles"},
                    new Category(){Id = 3, Name="Greeting Cards"},
                    new Category(){Id = 4, Name="Dolls"},
                    new Category(){Id = 5, Name="Files"},
                    new Category(){Id = 6, Name="Hand Bags"},
                    new Category(){Id = 7, Name="Wallets"},
                    ]);
            });




        }

    }
}
