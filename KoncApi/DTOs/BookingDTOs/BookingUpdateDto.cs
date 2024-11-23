using System;
namespace KoncApi;

 public class BookingUpdateDto
    {
        public required Guid VenueId { get; set; }
        public required Guid UserId { get; set; }
        public DateTimeOffset BookingDate { get; set; }
        public BookingStatus Status { get; set; }
    }