using System;
using System.Collections.Generic;

namespace KoncApi
{
    public interface IBookingService
    {
        List<BookingReadDto> GetAllBookings();
        BookingReadDto GetBookingById(Guid id);
        void AddBooking(BookingCreateDto bookingCreateDto);
        void UpdateBooking(Guid id, BookingUpdateDto bookingUpdateDto);
        void DeleteBooking(Guid id);
    }
}
