using GraphQL.Types;
namespace KoncApi;

public class VenueType : ObjectGraphType<Venue>
{
    public VenueType()
    {
        Field(x => x.Id, type: typeof(IdGraphType)).Description("ID of the venue.");
        Field(x => x.Name).Description("Name of the venue.");
        Field(x => x.Location).Description("Location of the venue.");
        Field(x => x.Capacity).Description("Capacity of the venue.");
        Field(x => x.AvailableDates, type: typeof(ListGraphType<DateTimeGraphType>)).Description("Available dates for the venue.");
    }
}
