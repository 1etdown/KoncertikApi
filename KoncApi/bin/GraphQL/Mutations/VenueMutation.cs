using GraphQL;
using GraphQL.Types;

namespace KoncApi;

public class VenueMutation : ObjectGraphType
{
    
    public VenueMutation(IVenueService venueService)
    {
        Field<VenueType>(
            "createVenue",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<VenueCreateInputType>> { Name = "venue" }
            ),
            resolve: context =>
            {
                var venue = context.GetArgument<VenueCreateDto>("venue");
                venueService.AddVenue(venue);
                return venue;
            }
        );

        Field<VenueType>(
            "updateVenue",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" },
                new QueryArgument<NonNullGraphType<VenueUpdateInputType>> { Name = "venue" }
            ),
            resolve: context =>
            {
                var id = context.GetArgument<Guid>("id");
                var venue = context.GetArgument<VenueUpdateDto>("venue");
                venueService.UpdateVenue(id, venue);
                return venueService.GetVenueById(id);
            }
        );
    }
}
