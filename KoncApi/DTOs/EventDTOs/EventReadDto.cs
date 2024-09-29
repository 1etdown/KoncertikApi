using System;
namespace KoncApi;

   public class EventReadDto
    {
        public Guid Id { get; set; }
        public Guid BookingId { get; set; }
        public string EventName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int TicketsAvailable { get; set; }
    }