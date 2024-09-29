using System;
using System.Collections.Generic;
using System.Linq;

namespace KoncApi
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;

        public BookingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<BookingReadDto> GetAllBookings()
        {
            return _context.Bookings
                .Select(b => new BookingReadDto
                {
                    Id = b.Id,
                    VenueId = b.VenueId,
                    UserId = b.UserId,
                    BookingDate = b.BookingDate,
                    Status = b.Status
                })
                .ToList();
        }

        public BookingReadDto GetBookingById(Guid id)
        {
            var booking = _context.Bookings.Find(id);

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

        public void AddBooking(BookingCreateDto bookingCreateDto)
        {
            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                VenueId = bookingCreateDto.VenueId,
                UserId = bookingCreateDto.UserId,
                BookingDate = bookingCreateDto.BookingDate,
                Status = bookingCreateDto.Status
            };

            _context.Bookings.Add(booking);
            _context.SaveChanges();
        }

        public void UpdateBooking(Guid id, BookingUpdateDto bookingUpdateDto)
        {
            var booking = _context.Bookings.Find(id);
            if (booking == null) return;

            booking.VenueId = bookingUpdateDto.VenueId;
            booking.UserId = bookingUpdateDto.UserId;
            booking.BookingDate = bookingUpdateDto.BookingDate;
            booking.Status = bookingUpdateDto.Status;

            _context.SaveChanges();
        }

        public void DeleteBooking(Guid id)
        {
            var booking = _context.Bookings.Find(id);
            if (booking == null) return;

            _context.Bookings.Remove(booking);
            _context.SaveChanges();
        }
    }
}
