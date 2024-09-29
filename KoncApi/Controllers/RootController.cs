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
            var entity = "venues";

            return Ok(
                new
                {
                    self = new { href = Url.Link("Root", null),
                                 description = "Wellcome to KoncApi."},
                    entity,
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
                }
            );
        }
    }
}
