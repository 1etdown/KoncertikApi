using Microsoft.EntityFrameworkCore;
using HotChocolate.Types;
namespace KoncApi;
[ExtendObjectType(name: "Mutation")]
public class BookingMutation
{
    private readonly ApplicationDbContext _context;

    public BookingMutation(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BookingReadDto?> CreateBookingAsync(BookingCreateDto newBookingDto)
    {
        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            VenueId = newBookingDto.VenueId,
            UserId = newBookingDto.UserId,
            BookingDate = newBookingDto.BookingDate,
            Status = newBookingDto.Status
        };

        await _context.Bookings.AddAsync(booking);
        await _context.SaveChangesAsync();

        return new BookingReadDto
        {
            Id = booking.Id,
            VenueId = booking.VenueId,
            UserId = booking.UserId,
            BookingDate = booking.BookingDate,
            Status = booking.Status
        };
    }

    public async Task<BookingReadDto?> UpdateBookingAsync(Guid id, BookingUpdateDto bookingUpdateDto)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking == null) return null;

        booking.VenueId = bookingUpdateDto.VenueId;
        booking.UserId = bookingUpdateDto.UserId;
        booking.BookingDate = bookingUpdateDto.BookingDate;
        booking.Status = bookingUpdateDto.Status;

        await _context.SaveChangesAsync();

        return new BookingReadDto
        {
            Id = booking.Id,
            VenueId = booking.VenueId,
            UserId = booking.UserId,
            BookingDate = booking.BookingDate,
            Status = booking.Status
        };
    }

    public async Task<bool> DeleteBookingAsync(Guid id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking == null) return false;

        _context.Bookings.Remove(booking);
        await _context.SaveChangesAsync();
        return true;
    }
}
