using Microsoft.AspNetCore.Mvc;
using System;

namespace KoncApi
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet("GetAllEvents", Name = "GetAllEvents")]
        public IActionResult GetAllEvents()
        {
            var events = _eventService.GetAllEvents();
            return Ok(events);
        }

        [HttpGet("{id}", Name = "GetEventById")]
        public IActionResult GetEventById(Guid id)
        {
            var eventDetails = _eventService.GetEventById(id);
            if (eventDetails == null)
            {
                return NotFound();
            }
            return Ok(eventDetails);
        }

        [HttpPost("AddEvent", Name = "AddEvent")]
        public IActionResult AddEvent([FromBody] EventCreateDto eventCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var eventEntity = new Event
            {
                Id = Guid.NewGuid(),
                BookingId = eventCreateDto.BookingId,
                EventName = eventCreateDto.EventName,
                StartTime = eventCreateDto.StartTime,
                EndTime = eventCreateDto.EndTime,
                TicketsAvailable = eventCreateDto.TicketsAvailable
            };

            _eventService.AddEvent(eventCreateDto);
            return CreatedAtAction(nameof(GetEventById), new { id = eventEntity.Id }, eventEntity);
        }

        [HttpPut("{id}", Name = "UpdateEvent")]
        public IActionResult UpdateEvent(Guid id, [FromBody] EventUpdateDto eventUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var eventEntity = _eventService.GetEventById(id);
            if (eventEntity == null)
            {
                return NotFound();
            }

            _eventService.UpdateEvent(id, eventUpdateDto);
            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteEvent")]
        public IActionResult DeleteEvent(Guid id)
        {
            var eventEntity = _eventService.GetEventById(id);
            if (eventEntity == null)
            {
                return NotFound();
            }

            _eventService.DeleteEvent(id);
            return NoContent();
        }
    }
}
