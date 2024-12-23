using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace KoncAPI
{
    
    public class BookingHub : Hub
    {
        
        public async Task SendBookingStatus(string bookingId, string status)
        {
            await Clients.All.SendAsync("BookingStatusChanged", bookingId, status);
        }
    }
}

