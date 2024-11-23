using GraphQL.Types;

namespace KoncApi;

public class VenueQuery : ObjectGraphType
{
    public VenueQuery(IVenueService venueService)
    {
        Field<ListGraphType<VenueType>>(
            "venues",
            resolve: context => venueService.GetAllVenues()
        );

        Field<VenueType>(
            "venue",
            arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }),
            resolve: context => venueService.GetVenueById(context.GetArgument<Guid>("id"))
        );
    }
}
