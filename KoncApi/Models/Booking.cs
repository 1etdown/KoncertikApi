using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace KoncApi

{
public class Booking
{
    [Key]
    public Guid Id { get; set; }
    [ForeignKey("VenueId")]
    public Guid VenueId { get; set; }
    [ForeignKey("UserId")]
    public Guid UserId { get; set; }
    public DateTime BookingDate { get; set; }
    public BookingStatus Status { get; set; }
}
}

