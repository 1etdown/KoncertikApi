using System;
namespace KoncApi;

public class BookingCreateDto
    {
        public required Guid VenueId { get; set; }
        public required Guid UserId { get; set; }
        public DateTimeOffset BookingDate { get; set; }
        public BookingStatus Status { get; set; }
    }