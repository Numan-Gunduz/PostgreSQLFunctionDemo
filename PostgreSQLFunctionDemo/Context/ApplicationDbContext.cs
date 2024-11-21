using Microsoft.EntityFrameworkCore;
using PostgreSQLFunctionDemo.Models;

namespace PostgreSQLFunctionDemo.Context
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User ve Order arasında bir ilişki tanımlıyoruz
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);
           
        }
        public void SeedData()
        {
            if (!Users.Any())
            {
                Users.AddRange(
                    new User { Id = 1, FirstName = "Numan", LastName = "Gündüz" },
                    new User { Id = 2, FirstName = "Ahmet", LastName = "Çetin" }
                );
            }

            if (!Orders.Any())
            {
                Orders.AddRange(
                    new Order
                    {
                        Id = 1,
                        UserId = 1,
                        Amount = 100.50m,
                        OrderDate = DateTime.SpecifyKind(new DateTime(2024, 11, 15), DateTimeKind.Utc)
                    },
                    new Order
                    {
                        Id = 2,
                        UserId = 1,
                        Amount = 250.75m,
                        OrderDate = DateTime.SpecifyKind(new DateTime(2024, 11, 18), DateTimeKind.Utc)
                    },
                    new Order
                    {
                        Id = 3,
                        UserId = 2,
                        Amount = 300.00m,
                        OrderDate = DateTime.SpecifyKind(new DateTime(2024, 11, 19), DateTimeKind.Utc)
                    }
                );
            }

            SaveChanges();
        }

    }
}
