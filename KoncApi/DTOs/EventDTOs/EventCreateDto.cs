using System;

namespace KoncApi;

 public class EventCreateDto
    {
        public required Guid BookingId { get; set; }
        public required string EventName { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public int TicketsAvailable { get; set; }
    }