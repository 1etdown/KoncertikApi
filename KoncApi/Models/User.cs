using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KoncApi
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public required string Surname { get; set; }

        public long PhoneNumber { get; set; }

        public required string Email { get; set; }

        public ICollection<Booking> Bookings { get; set; }
    }
}
