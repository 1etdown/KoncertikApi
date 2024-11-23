using System;
using System.Collections.Generic;
using System.Linq;

namespace KoncApi
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;
        private readonly IRabbitMqService _rabbitMqService;

        public BookingService(ApplicationDbContext context, IRabbitMqService rabbitMqService)
        {
            _context = context;
            _rabbitMqService = rabbitMqService ?? throw new ArgumentNullException(nameof(rabbitMqService));
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
            var isVenueAvailable = !_context.Bookings
                .Any(b => b.VenueId == bookingCreateDto.VenueId && b.BookingDate == bookingCreateDto.BookingDate);

            if (!isVenueAvailable)
            {
                throw new InvalidOperationException("Venue is not available on the selected date.");
            }

            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                VenueId = bookingCreateDto.VenueId,
                UserId = bookingCreateDto.UserId,
                BookingDate = bookingCreateDto.BookingDate,
                Status = bookingCreateDto.Status
            };

            var venue = _context.Venues.Find(bookingCreateDto.VenueId);
            if (venue == null)
            {
                throw new InvalidOperationException("Venue not found.");
            }

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            // Send booking information and venue name to RabbitMQ
            _rabbitMqService.SendMessage(new
            {
                booking.Id,
                booking.VenueId,
                booking.UserId,
                booking.BookingDate,
                Status = booking.Status.ToString(),
                VenueName = venue.Name
            });
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
