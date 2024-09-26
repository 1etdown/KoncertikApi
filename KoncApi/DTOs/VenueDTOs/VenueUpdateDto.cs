using System;
using System.Collections.Generic;

namespace KoncApi
{
    public class VenueUpdateDto
    {
        public required string Name { get; set; }
        public required string Location { get; set; }
        public int Capacity { get; set; }
        public required List<DateTime> AvailableDates { get; set; }
    }
}
