using Microsoft.AspNetCore.Mvc;
using System;

namespace KoncApi
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("GetAllBookings", Name = "GetAllBookings")]
        public IActionResult GetAllBookings()
        {
            var bookings = _bookingService.GetAllBookings();
            return Ok(bookings);
        }

        [HttpGet("{id}", Name = "GetBookingById")]
        public IActionResult GetBookingById(Guid id)
        {
            var booking = _bookingService.GetBookingById(id);
            if (booking == null)
            {
                return NotFound();
            }
            return Ok(booking);
        }

        [HttpPost("AddBooking", Name = "AddBooking")]
        public IActionResult AddBooking([FromBody] BookingCreateDto bookingCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                VenueId = bookingCreateDto.VenueId,
                UserId = bookingCreateDto.UserId,
                BookingDate = bookingCreateDto.BookingDate,
                Status = bookingCreateDto.Status
            };

            _bookingService.AddBooking(bookingCreateDto);
            return CreatedAtAction(nameof(GetBookingById), new { id = booking.Id }, booking);
        }

        [HttpPut("{id}", Name = "UpdateBooking")]
        public IActionResult UpdateBooking(Guid id, [FromBody] BookingUpdateDto bookingUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var booking = _bookingService.GetBookingById(id);
            if (booking == null)
            {
                return NotFound();
            }

            _bookingService.UpdateBooking(id, bookingUpdateDto);
            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteBooking")]
        public IActionResult DeleteBooking(Guid id)
        {
            var booking = _bookingService.GetBookingById(id);
            if (booking == null)
            {
                return NotFound();
            }

            _bookingService.DeleteBooking(id);
            return NoContent();
        }
    }
}
