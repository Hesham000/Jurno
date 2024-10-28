using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Travel.Models;

namespace Travel.Data
{
    public class TravelDbContext : IdentityDbContext<ApplicationUser>
    {
        public TravelDbContext(DbContextOptions<TravelDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tour> Tours { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<TravelGuide> TravelGuides { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Tour)
                .WithMany(t => t.Bookings)
                .HasForeignKey(b => b.TourId);

            // Configure decimal precision for price and discount
            modelBuilder.Entity<Tour>()
                .Property(t => t.Price).HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Tour>()
                .Property(t => t.Discount).HasColumnType("decimal(5,2)");

            // Ensure Tour maps to the correct table
            modelBuilder.Entity<Tour>().ToTable("Tours");
        }
    }
}
