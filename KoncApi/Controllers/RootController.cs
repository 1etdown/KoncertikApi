using Microsoft.AspNetCore.Mvc;

namespace KoncApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class RootController : ControllerBase
    {
        [HttpGet("/", Name = "Root")]
        public IActionResult GetRoot()
        {
            return Ok(
                new
                {
                    self = new
                    {
                        href = Url.Link("Root", null),
                        description = "Welcome to KoncApi."
                    },
                    entities = new[]
                    {
                        GetVenueLinks(),
                        GetEventLinks(),
                        GetUserLinks(),
                        GetBookingLinks()
                    }
                }
            );
        }

        private object GetVenueLinks()
        {
            return new
            {
                entity = "venues",
                _actions = new
                {
                    getAll = new
                    {
                        href = Url.Link("GetAllVenues", null),
                        method = "GET",
                        description = "Retrieve all venues."
                    },
                    getById = new
                    {
                        href = Url.Link("GetVenueById", new { id = "someId" }),
                        method = "GET",
                        description = "Retrieve a venue by ID."
                    },
                    create = new
                    {
                        href = Url.Link("AddVenue", null),
                        method = "POST",
                        description = "Add a new venue."
                    },
                    update = new
                    {
                        href = Url.Link("UpdateVenue", new { id = "someId" }),
                        method = "PUT",
                        description = "Update a venue by ID."
                    },
                    delete = new
                    {
                        href = Url.Link("DeleteVenue", new { id = "someId" }),
                        method = "DELETE",
                        description = "Delete a venue by ID."
                    }
                }
            };
        }

        private object GetEventLinks()
        {
            return new
            {
                entity = "events",
                _actions = new
                {
                    getAll = new
                    {
                        href = Url.Link("GetAllEvents", null),
                        method = "GET",
                        description = "Retrieve all events."
                    },
                    getById = new
                    {
                        href = Url.Link("GetEventById", new { id = "someId" }),
                        method = "GET",
                        description = "Retrieve an event by ID."
                    },
                    create = new
                    {
                        href = Url.Link("AddEvent", null),
                        method = "POST",
                        description = "Add a new event."
                    },
                    update = new
                    {
                        href = Url.Link("UpdateEvent", new { id = "someId" }),
                        method = "PUT",
                        description = "Update an event by ID."
                    },
                    delete = new
                    {
                        href = Url.Link("DeleteEvent", new { id = "someId" }),
                        method = "DELETE",
                        description = "Delete an event by ID."
                    }
                }
            };
        }

        private object GetUserLinks()
        {
            return new
            {
                entity = "users",
                _actions = new
                {
                    getAll = new
                    {
                        href = Url.Link("GetAllUsers", null),
                        method = "GET",
                        description = "Retrieve all users."
                    },
                    getById = new
                    {
                        href = Url.Link("GetUserById", new { id = "someId" }),
                        method = "GET",
                        description = "Retrieve a user by ID."
                    },
                    create = new
                    {
                        href = Url.Link("AddUser", null),
                        method = "POST",
                        description = "Add a new user."
                    },
                    update = new
                    {
                        href = Url.Link("UpdateUser", new { id = "someId" }),
                        method = "PUT",
                        description = "Update a user by ID."
                    },
                    delete = new
                    {
                        href = Url.Link("DeleteUser", new { id = "someId" }),
                        method = "DELETE",
                        description = "Delete a user by ID."
                    }
                }
            };
        }

        private object GetBookingLinks()
        {
            return new
            {
                entity = "bookings",
                _actions = new
                {
                    getAll = new
                    {
                        href = Url.Link("GetAllBookings", null),
                        method = "GET",
                        description = "Retrieve all bookings."
                    },
                    getById = new
                    {
                        href = Url.Link("GetBookingById", new { id = "someId" }),
                        method = "GET",
                        description = "Retrieve a booking by ID."
                    },
                    create = new
                    {
                        href = Url.Link("AddBooking", null),
                        method = "POST",
                        description = "Add a new booking."
                    },
                    update = new
                    {
                        href = Url.Link("UpdateBooking", new { id = "someId" }),
                        method = "PUT",
                        description = "Update a booking by ID."
                    },
                    delete = new
                    {
                        href = Url.Link("DeleteBooking", new { id = "someId" }),
                        method = "DELETE",
                        description = "Delete a booking by ID."
                    }
                }
            };
        }
    }
}
