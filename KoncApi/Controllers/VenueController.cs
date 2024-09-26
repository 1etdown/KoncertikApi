using Microsoft.AspNetCore.Mvc;
using System;
namespace KoncApi

{

[ApiController]
[Route("api/[controller]")]
public class VenueController : ControllerBase
{
    private readonly IVenueService _venueService;

    public VenueController(IVenueService venueService)
    {
        _venueService = venueService;
    }

    [HttpGet("GetAllVenues", Name = "GetAllVenues")]
    public IActionResult GetAllVenues()
    {
        var venues = _venueService.GetAllVenues();
        return Ok(venues);
    }

    [HttpGet("{id}", Name = "GetVenueById")]
    public IActionResult GetVenueById(Guid id)
    {
        var venue = _venueService.GetVenueById(id);
        if (venue == null)
        {
            return NotFound();
        }
        return Ok(venue);
    }

    [HttpPost("AddVenue",Name = "AddVenue")]
    public IActionResult AddVenue([FromBody] VenueCreateDto venueCreateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var venue = new Venue
        {
            Id = Guid.NewGuid(),
            Name = venueCreateDto.Name,
            Location = venueCreateDto.Location,
            Capacity = venueCreateDto.Capacity,
            AvailableDates = venueCreateDto.AvailableDates
        };

        _venueService.AddVenue(venueCreateDto);
        return CreatedAtAction(nameof(GetVenueById), new { id = venue.Id }, venue);
    }

    [HttpPut("{id}", Name = "UpdateVenue")]
    public IActionResult UpdateVenue(Guid id, [FromBody] VenueUpdateDto venueUpdateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var venue = _venueService.GetVenueById(id);
        if (venue == null)
        {
            return NotFound();
        }

        _venueService.UpdateVenue(id, venueUpdateDto);
        return NoContent();
    }

    [HttpDelete("{id}", Name = "DeleteVenue")]
    public IActionResult DeleteVenue(Guid id)
    {
        var venue = _venueService.GetVenueById(id);
        if (venue == null)
        {
            return NotFound();
        }

        _venueService.DeleteVenue(id);
        return NoContent();
    }
}

}