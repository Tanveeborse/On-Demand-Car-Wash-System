using GreenWashAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Data
{


public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Explicitly configure the relationship between Order and User (as a customer)
        modelBuilder.Entity<Order>()
            .HasOne(o => o.User) // Order has one User (as a customer)
            .WithMany(u => u.OrderUsers) // User can have many Orders (as a customer)
            .HasForeignKey(o => o.UserId) // The foreign key is UserId in Order
            .OnDelete(DeleteBehavior.SetNull); // Optional: Specify delete behavior

        // Explicitly configure the relationship between Order and User (as a washer)
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Washer) // Order has one User (as a washer)
            .WithMany(u => u.OrderWashers) // User can have many Orders (as a washer)
            .HasForeignKey(o => o.WasherId) // The foreign key is WasherId in Order
            .OnDelete(DeleteBehavior.SetNull); // Optional: Specify delete behavior

            // If you want to ignore the navigation properties that are causing issues, you can explicitly ignore them
            modelBuilder.Entity<User>()
                .Ignore(u => u.OrderUsers)  // Ignore the navigation property to OrderUsers in User entity
                .Ignore(u => u.OrderWashers);  // Ignore the navigation property to OrderWashers in User entity

            // Optional: If you don't need to map any other navigation properties, you can ignore them as well

            // Configure other relationships as necessary

            modelBuilder.Entity<Review>()
       .HasOne(r => r.User)  // A Review has one User
       .WithMany(u => u.ReviewUsers)  // A User can have many Reviews
       .HasForeignKey(r => r.UserId)  // Foreign key is UserId in Review
       .OnDelete(DeleteBehavior.Cascade);
    }

    // DbSets for your entities
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<Leaderboard> Leaderboards { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<OrderAddOn> OrderAddOns { get; set; }
    public DbSet<Package> Packages { get; set; }
    public DbSet<Payment> Payments { get; set; }
}
}
