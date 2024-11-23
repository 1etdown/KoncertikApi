
using Microsoft.EntityFrameworkCore;
using HotChocolate.Types;
namespace KoncApi;
[ExtendObjectType(name:"Mutation")]
public class VenueMutation
{
    private readonly ApplicationDbContext _context;

    public VenueMutation(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<VenueReadDto?> CreateVenueAsync(VenueCreateDto newVenueDto)
    {
        var venue = new Venue
        {
            Id = Guid.NewGuid(),
            Name = newVenueDto.Name,
            Location = newVenueDto.Location,
            Capacity = newVenueDto.Capacity,
        };

        await _context.Venues.AddAsync(venue);
        await _context.SaveChangesAsync();

        return new VenueReadDto
        {
            Id = venue.Id,
            Name = venue.Name,
            Location = venue.Location,
            Capacity = venue.Capacity,
        };
    }

    public async Task<VenueReadDto?> UpdateVenueAsync(Guid id, VenueUpdateDto venueUpdateDto)
    {
        var venue = await _context.Venues.FindAsync(id);
        if (venue == null) return null;

        venue.Name = venueUpdateDto.Name;
        venue.Location = venueUpdateDto.Location;
        venue.Capacity = venueUpdateDto.Capacity;
  

        await _context.SaveChangesAsync();

        return new VenueReadDto
        {
            Id = venue.Id,
            Name = venue.Name,
            Location = venue.Location,
            Capacity = venue.Capacity,

        };
    }

    public async Task<bool> DeleteVenueAsync(Guid id)
    {
        var venue = await _context.Venues.FindAsync(id);
        if (venue == null) return false;

        _context.Venues.Remove(venue);
        await _context.SaveChangesAsync();
        return true;
    }
}
