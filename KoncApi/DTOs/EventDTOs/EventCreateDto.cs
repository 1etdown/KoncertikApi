using System;

namespace KoncApi;

 public class EventCreateDto
    {
        public required Guid BookingId { get; set; }
        public required string EventName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int TicketsAvailable { get; set; }
    }