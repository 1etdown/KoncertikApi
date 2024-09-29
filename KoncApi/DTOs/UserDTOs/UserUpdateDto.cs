using System;
namespace KoncApi;

public class UserUpdateDto
    {
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public int PhoneNumber { get; set; }
        public required string Email { get; set; }
    }