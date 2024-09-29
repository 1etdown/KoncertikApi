using System;
using System.Collections.Generic;

namespace KoncApi
{
    public interface IEventService
    {
        List<EventReadDto> GetAllEvents();
        EventReadDto GetEventById(Guid id);
        void AddEvent(EventCreateDto eventCreateDto);
        void UpdateEvent(Guid id, EventUpdateDto eventUpdateDto);
        void DeleteEvent(Guid id);
    }
}
