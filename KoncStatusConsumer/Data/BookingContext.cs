// using KoncStatusConsumer;
// using Microsoft.EntityFrameworkCore;
// namespace KoncStatusConsumer;
//
//
//
// public class BookingContext : DbContext
// {
//     public DbSet<Booking> Bookings { get; set; }
//
//     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//     {
//         optionsBuilder.UseNpgsql("Host=localhost;Database=statusdbdb;Username=pguser;Password=0000");
//     }
// }