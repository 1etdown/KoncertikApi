 using KoncStatusConsumer;

using Microsoft.EntityFrameworkCore;

public class BookingContext : DbContext
{
    public DbSet<Booking> Bookings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5444;Database=statusdb;Username=pguser;Password=0000");
    }
}