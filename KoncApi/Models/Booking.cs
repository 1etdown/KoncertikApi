using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KoncApi
{
    public class Booking
    {
        [Key]
        public Guid Id { get; set; }

        public Guid VenueId { get; set; }
        
        [ForeignKey("VenueId")]
        public Venue Venue { get; set; }
        
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public DateTime BookingDate { get; set; }
        public BookingStatus Status { get; set; }
        public ICollection<Event> Events { get; set; }
    }
}
