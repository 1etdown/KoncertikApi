using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KoncApi
{
    public class Event
    {
        [Key]
        public Guid Id { get; set; }
        public Guid BookingId { get; set; }
        
        [ForeignKey("BookingId")]
        public Booking Booking { get; set; }
        public required string EventName { get; set; }

        public DateTimeOffset  StartTime { get; set; }
        public DateTimeOffset  EndTime { get; set; }
        public int TicketsAvailable { get; set; }
    }
}
