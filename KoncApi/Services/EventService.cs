using System;
using System.Collections.Generic;
using System.Linq;

namespace KoncApi
{
    public class EventService : IEventService
    {
        private readonly ApplicationDbContext _context;

        public EventService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<EventReadDto> GetAllEvents()
        {
            return _context.Events
                .Select(e => new EventReadDto
                {
                    Id = e.Id,
                    BookingId = e.BookingId,
                    EventName = e.EventName,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                    TicketsAvailable = e.TicketsAvailable
                })
                .ToList();
        }

        public EventReadDto GetEventById(Guid id)
        {
            var eventEntity = _context.Events.Find(id);

            if (eventEntity == null) return null;

            return new EventReadDto
            {
                Id = eventEntity.Id,
                BookingId = eventEntity.BookingId,
                EventName = eventEntity.EventName,
                StartTime = eventEntity.StartTime,
                EndTime = eventEntity.EndTime,
                TicketsAvailable = eventEntity.TicketsAvailable
            };
        }

        public void AddEvent(EventCreateDto eventCreateDto)
        {
            var eventEntity = new Event
            {
                Id = Guid.NewGuid(),
                BookingId = eventCreateDto.BookingId,
                EventName = eventCreateDto.EventName,
                StartTime = eventCreateDto.StartTime,
                EndTime = eventCreateDto.EndTime,
                TicketsAvailable = eventCreateDto.TicketsAvailable
            };

            _context.Events.Add(eventEntity);
            _context.SaveChanges();
        }

        public void UpdateEvent(Guid id, EventUpdateDto eventUpdateDto)
        {
            var eventEntity = _context.Events.Find(id);
            if (eventEntity == null) return;

            eventEntity.BookingId = eventUpdateDto.BookingId;
            eventEntity.EventName = eventUpdateDto.EventName;
            eventEntity.StartTime = eventUpdateDto.StartTime;
            eventEntity.EndTime = eventUpdateDto.EndTime;
            eventEntity.TicketsAvailable = eventUpdateDto.TicketsAvailable;

            _context.SaveChanges();
        }

        public void DeleteEvent(Guid id)
        {
            var eventEntity = _context.Events.Find(id);
            if (eventEntity == null) return;

            _context.Events.Remove(eventEntity);
            _context.SaveChanges();
        }
    }
}
