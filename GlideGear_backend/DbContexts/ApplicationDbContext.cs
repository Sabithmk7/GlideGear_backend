using GlideGear_backend.Models;
using GlideGear_backend.Models.Order_Model;
using Microsoft.EntityFrameworkCore;

namespace GlideGear_backend.DbContexts
{
    public class ApplicationDbContext:DbContext
    {
        private readonly string connectionString;
        public ApplicationDbContext(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:DefaultConnection"];
        }

        //DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; } 
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(e => e.Role)
                .HasDefaultValue("user");
            modelBuilder.Entity<Product>()
                .Property(pr => pr.Price).
                HasPrecision(18, 2);
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(c => c.Category)
                .HasForeignKey(x => x.CategoryId);
            modelBuilder.Entity<User>()
              .HasOne(u => u.Cart)
              .WithOne(c => c.User)
              .HasForeignKey<Cart>(c => c.UserId);
            modelBuilder.Entity<Cart>()
                .HasMany(x=>x.CartItems)
                .WithOne(c=>c.Cart)
                .HasForeignKey(i=>i.CartId);
            modelBuilder.Entity<CartItem>()
                .HasOne(c=>c.Product)
                .WithMany(i => i.CartItems)
                .HasForeignKey(k=>k.ProductId);
                
                  



        }
    }
}
