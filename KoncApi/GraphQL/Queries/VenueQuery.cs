using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace KoncApi;
[ExtendObjectType(name:"Query")]
public class VenueQuery
{
    private readonly ApplicationDbContext _context;

    public VenueQuery(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<VenueReadDto>> GetAllVenuesAsync()
    {
        return await _context.Venues
            .Select(v => new VenueReadDto
            {
                Id = v.Id,
                Name = v.Name,
                Location = v.Location,
                Capacity = v.Capacity,
 
            })
            .ToListAsync();
    }

    public async Task<VenueReadDto?> GetVenueByIdAsync(Guid id)
    {
        var venue = await _context.Venues.FindAsync(id);
        if (venue == null) return null;

        return new VenueReadDto
        {
            Id = venue.Id,
            Name = venue.Name,
            Location = venue.Location,
            Capacity = venue.Capacity,
      
        };
    }
}
