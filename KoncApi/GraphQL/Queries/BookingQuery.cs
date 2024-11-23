using Microsoft.EntityFrameworkCore;
using HotChocolate.Types;

namespace KoncApi;
[ExtendObjectType(name: "Query")]
public class BookingQuery
{
    private readonly ApplicationDbContext _context;

    public BookingQuery(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<BookingReadDto>> GetAllBookingsAsync()
    {
        return await _context.Bookings
            .Select(b => new BookingReadDto
            {
                Id = b.Id,
                VenueId = b.VenueId,
                UserId = b.UserId,
                BookingDate = b.BookingDate,
                Status = b.Status
            })
            .ToListAsync();
    }
      
    public async Task<BookingReadDto?> GetBookingByIdAsync(Guid id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking == null) return null;

        return new BookingReadDto
        {
            Id = booking.Id,
            VenueId = booking.VenueId,
            UserId = booking.UserId,
            BookingDate = booking.BookingDate,
            Status = booking.Status
        };
    }
}
