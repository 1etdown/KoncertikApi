using Microsoft.EntityFrameworkCore;

namespace KoncApi
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Booking> Booking { get; set; }

        public DbSet<Venue> Venues { get; set; } // Add this line
    }
}