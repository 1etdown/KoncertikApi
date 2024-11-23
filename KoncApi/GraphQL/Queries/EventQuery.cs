using Microsoft.EntityFrameworkCore;
using HotChocolate.Types;

namespace KoncApi;
[ExtendObjectType(name: "Query")]
public class EventQuery
{
    private readonly ApplicationDbContext _context;

    public EventQuery(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<EventReadDto>> GetAllEventsAsync()
    {
        return await _context.Events
            .Select(e => new EventReadDto
            {
                Id = e.Id,
                BookingId = e.BookingId,
                EventName = e.EventName,
                StartTime = e.StartTime,
                EndTime = e.EndTime,
                TicketsAvailable = e.TicketsAvailable
            })
            .ToListAsync();
    }

    public async Task<EventReadDto?> GetEventByIdAsync(Guid id)
    {
        var eventObj = await _context.Events.FindAsync(id);
        if (eventObj == null) return null;

        return new EventReadDto
        {
            Id = eventObj.Id,
            BookingId = eventObj.BookingId,
            EventName = eventObj.EventName,
            StartTime = eventObj.StartTime,
            EndTime = eventObj.EndTime,
            TicketsAvailable = eventObj.TicketsAvailable
        };
    }
}
