namespace KoncConsumer;

public class BookingInfo
{
    public Guid Id { get; set; }
    public Guid VenueId { get; set; }
    public Guid UserId { get; set; }
    public DateTime BookingDate { get; set; }
    public string Status { get; set; }
    public string VenueName { get; set; }
}
