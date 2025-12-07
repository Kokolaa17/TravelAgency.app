using LuxTravel.app.Models;
using Microsoft.EntityFrameworkCore;

namespace LuxTravel.app.Data;

public class DataContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Agency> Agencies { get; set; }
    public DbSet<Tour> Tours { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<AgencyReview> AgencyReviews { get; set; }
    public DbSet<Wishlist> Wishlists { get; set; }
    public DbSet<Admin> Admins { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Configure your connection string here
        optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LuxTravel.cs;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User-Agency one-to-one relationship
        // Agency is dependent (has the foreign key OwnerId)
        modelBuilder.Entity<Agency>()
            .HasOne(a => a.Owner)
            .WithOne(u => u.OwnedAgency)
            .HasForeignKey<Agency>(a => a.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Agency-Tour relationship
        modelBuilder.Entity<Tour>()
            .HasOne(t => t.Agency)
            .WithMany(a => a.Tours)
            .HasForeignKey(t => t.AgencyId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure User-Booking relationship
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Tour-Booking relationship
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Tour)
            .WithMany(t => t.Bookings)
            .HasForeignKey(b => b.TourId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure User-Review relationship
        modelBuilder.Entity<Review>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Tour-Review relationship
        modelBuilder.Entity<Review>()
            .HasOne(r => r.Tour)
            .WithMany(t => t.Reviews)
            .HasForeignKey(r => r.TourId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure User-AgencyReview relationship
        modelBuilder.Entity<AgencyReview>()
            .HasOne(ar => ar.User)
            .WithMany(u => u.AgencyReviews)
            .HasForeignKey(ar => ar.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Agency-AgencyReview relationship
        modelBuilder.Entity<AgencyReview>()
            .HasOne(ar => ar.Agency)
            .WithMany(a => a.Reviews)
            .HasForeignKey(ar => ar.AgencyId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure User-Wishlist relationship
        modelBuilder.Entity<Wishlist>()
            .HasOne(w => w.User)
            .WithMany(u => u.Wishlists)
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Tour-Wishlist relationship
        modelBuilder.Entity<Wishlist>()
            .HasOne(w => w.Tour)
            .WithMany(t => t.Wishlists)
            .HasForeignKey(w => w.TourId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure composite unique index for Wishlist (prevent duplicate wishlists)
        modelBuilder.Entity<Wishlist>()
            .HasIndex(w => new { w.UserId, w.TourId })
            .IsUnique();

        // Configure composite unique index for Review (one review per user per tour)
        modelBuilder.Entity<Review>()
            .HasIndex(r => new { r.UserId, r.TourId })
            .IsUnique();

        // Configure composite unique index for AgencyReview (one review per user per agency)
        modelBuilder.Entity<AgencyReview>()
            .HasIndex(ar => new { ar.UserId, ar.AgencyId })
            .IsUnique();

        // Ignore calculated properties
        modelBuilder.Entity<Tour>()
            .Ignore(t => t.DurationDays)
            .Ignore(t => t.DurationNights)
            .Ignore(t => t.AvailableSpots);
    }
}
