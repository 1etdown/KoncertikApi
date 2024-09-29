using System;
namespace KoncApi;

   public class BookingReadDto
    {
        public Guid Id { get; set; }
        public Guid VenueId { get; set; }
        public Guid UserId { get; set; }
        public DateTime BookingDate { get; set; }
        public BookingStatus Status { get; set; }
    }