using System;
using System.Collections.Generic;

namespace KoncApi{

    public interface IVenueService
    {
        List<VenueReadDto> GetAllVenues();
        VenueReadDto GetVenueById(Guid id);
        void AddVenue(VenueCreateDto venueCreateDto);
        void UpdateVenue(Guid id, VenueUpdateDto venueUpdateDto);
        void DeleteVenue(Guid id);
    }
}
