using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace KoncApi{

    public class VenueService : IVenueService
    {
        private readonly ApplicationDbContext _context;

        public VenueService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<VenueReadDto> GetAllVenues()
        {
            return _context.Venues
                .Select(v => new VenueReadDto
                {
                    Id = v.Id,
                    Name = v.Name,
                    Location = v.Location,
                    Capacity = v.Capacity,
               
                })
                .ToList();
        }

        public VenueReadDto GetVenueById(Guid id)
        {
            var venue = _context.Venues.Find(id);

            if (venue == null) return null;

            return new VenueReadDto
            {
                Id = venue.Id,
                Name = venue.Name,
                Location = venue.Location,
                Capacity = venue.Capacity,
       
            };
        }

        public void AddVenue(VenueCreateDto venueCreateDto)
        {
            var venue = new Venue
            {
                Id = Guid.NewGuid(),
                Name = venueCreateDto.Name,
                Location = venueCreateDto.Location,
                Capacity = venueCreateDto.Capacity,
           
            };

            _context.Venues.Add(venue);
            _context.SaveChanges();
        }

        public void UpdateVenue(Guid id, VenueUpdateDto venueUpdateDto)
        {
            var venue = _context.Venues.Find(id);
            if (venue == null) return;

            venue.Name = venueUpdateDto.Name;
            venue.Location = venueUpdateDto.Location;
            venue.Capacity = venueUpdateDto.Capacity;
 

            _context.SaveChanges();
        }

        public void DeleteVenue(Guid id)
        {
            var venue = _context.Venues.Find(id);
            if (venue == null) return;

            _context.Venues.Remove(venue);
            _context.SaveChanges();
        }
    }

}