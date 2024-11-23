using System;
namespace KoncApi;

   public class EventReadDto
    {
        public Guid Id { get; set; }
        public Guid BookingId { get; set; }
        public string EventName { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public int TicketsAvailable { get; set; }
    }