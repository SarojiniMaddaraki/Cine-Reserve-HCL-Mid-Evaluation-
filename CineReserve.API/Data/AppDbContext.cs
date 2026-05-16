using CineReserve.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace CineReserve.API.Data
{
    

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Showtime> Showtimes { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<TicketDetail> TicketDetails { get; set; }
        public DbSet<Seat> Seats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔥 FIX DECIMAL PRECISION (IMPORTANT)
            modelBuilder.Entity<Booking>()
                .Property(b => b.TotalAmount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Showtime>()
                .Property(s => s.BasePrice)
                .HasPrecision(10, 2);

            modelBuilder.Entity<TicketDetail>()
                .Property(t => t.Price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<User>()
                .Property(u => u.CreditBalance)
                .HasPrecision(10, 2);

            // 🔥 UNIQUE SEAT CONSTRAINT
            modelBuilder.Entity<TicketDetail>()
                .HasIndex(t => new { t.ShowtimeId, t.RowNumber, t.SeatNumber })
                .IsUnique();
        }
    }
}
