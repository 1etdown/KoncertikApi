using System;
namespace KoncApi;

  public class UserReadDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public long PhoneNumber { get; set; }
        public string Email { get; set; }
    }