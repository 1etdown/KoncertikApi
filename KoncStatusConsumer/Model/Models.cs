

public class Booking
{
    private DateTime _bookingDate;
    public string Id { get; set; }
    public string VenueId { get; set; }
    public string UserId { get; set; }

    public DateTime BookingDate
    {
        get => _bookingDate;
        set => _bookingDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }

    public string Status { get; set; }
    public string VenueName { get; set; }
}