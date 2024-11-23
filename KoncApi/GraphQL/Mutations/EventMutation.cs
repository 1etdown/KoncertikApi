using Microsoft.EntityFrameworkCore;
using HotChocolate.Types;
namespace KoncApi;
[ExtendObjectType(name: "Mutation")]
public class EventMutation
{
    private readonly ApplicationDbContext _context;

    public EventMutation(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<EventReadDto?> CreateEventAsync(EventCreateDto newEventDto)
    {
        var eventObj = new Event
        {
            Id = Guid.NewGuid(),
            BookingId = newEventDto.BookingId,
            EventName = newEventDto.EventName,
            StartTime = newEventDto.StartTime,
            EndTime = newEventDto.EndTime,
            TicketsAvailable = newEventDto.TicketsAvailable
        };

        await _context.Events.AddAsync(eventObj);
        await _context.SaveChangesAsync();

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

    public async Task<EventReadDto?> UpdateEventAsync(Guid id, EventUpdateDto eventUpdateDto)
    {
        var eventObj = await _context.Events.FindAsync(id);
        if (eventObj == null) return null;

        eventObj.EventName = eventUpdateDto.EventName;
        eventObj.StartTime = eventUpdateDto.StartTime;
        eventObj.EndTime = eventUpdateDto.EndTime;
        eventObj.TicketsAvailable = eventUpdateDto.TicketsAvailable;

        await _context.SaveChangesAsync();

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

    public async Task<bool> DeleteEventAsync(Guid id)
    {
        var eventObj = await _context.Events.FindAsync(id);
        if (eventObj == null) return false;

        _context.Events.Remove(eventObj);
        await _context.SaveChangesAsync();
        return true;
    }
}
