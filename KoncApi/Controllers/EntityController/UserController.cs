using Microsoft.AspNetCore.Mvc;
using System;

namespace KoncApi
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetAllUsers", Name = "GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}", Name = "GetUserById")]
        public IActionResult GetUserById(Guid id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost("AddUser", Name = "AddUser")]
        public IActionResult AddUser([FromBody] UserCreateDto userCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = userCreateDto.Name,
                Surname = userCreateDto.Surname,
                PhoneNumber = userCreateDto.PhoneNumber,
                Email = userCreateDto.Email
            };

            _userService.AddUser(userCreateDto);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpPut("{id}", Name = "UpdateUser")]
        public IActionResult UpdateUser(Guid id, [FromBody] UserUpdateDto userUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            _userService.UpdateUser(id, userUpdateDto);
            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteUser")]
        public IActionResult DeleteUser(Guid id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            _userService.DeleteUser(id);
            return NoContent();
        }
    }
}
