using GraphQL.Types;

namespace KoncApi;

public class VenueSchema : Schema
{
    public VenueSchema(IServiceProvider provider) : base(provider)
    {
        Query = provider.GetRequiredService<VenueQuery>();
        Mutation = provider.GetRequiredService<VenueMutation>();
    }
}
