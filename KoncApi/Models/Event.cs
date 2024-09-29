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

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int TicketsAvailable { get; set; }
    }
}
