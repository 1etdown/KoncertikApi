using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KoncApi
{ 
    public class Venue
    {
        [Key]
        public Guid Id { get; set; }
        
        public required string Name { get; set; }
        public required string Location { get; set; }
        public int Capacity { get; set; }
        public required List<DateTime> AvailableDates { get; set; }
    }
}
