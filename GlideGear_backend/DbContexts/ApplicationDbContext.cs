using GlideGear_backend.Models;
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
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(e => e.Role)
                .HasDefaultValue("user");
        }

        public DbSet<User> Users { get; set; }

    }
}
