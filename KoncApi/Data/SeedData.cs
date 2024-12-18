using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KoncApi
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext context)
        {
            if (context.Users.Any() || context.Venues.Any() || context.Bookings.Any() || context.Events.Any())
            {
                return;
            }

            var users = new List<User>
            {
                new User
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "John",
                    Surname = "Doe",
                    PhoneNumber = 1234567890,
                    Email = "john.doe@example.com"
                },
                new User
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "Jane",
                    Surname = "Smith",
                    PhoneNumber = 9876543210,
                    Email = "jane.smith@example.com"
                }
            };

            context.Users.AddRange(users);
            context.SaveChanges();

            var venues = new List<Venue>
            {
                new Venue
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Name = "Concert Hall A",
                    Location = "New York",
                    Capacity = 500
                },
                new Venue
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Name = "Concert Hall B",
                    Location = "Los Angeles",
                    Capacity = 300
                }
            };

            context.Venues.AddRange(venues);
            context.SaveChanges();

            var bookings = new List<Booking>
            {
                new Booking
                {
                    Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    VenueId = venues[0].Id,
                    UserId = users[0].Id,
                    BookingDate = DateTimeOffset.Now.ToUniversalTime(),
                    Status = BookingStatus.Confirmed
                },
                new Booking
                {
                    Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                    VenueId = venues[1].Id,
                    UserId = users[1].Id,
                    BookingDate = DateTimeOffset.Now.ToUniversalTime(),
                    Status = BookingStatus.Pending
                }
            };

            context.Bookings.AddRange(bookings);
            context.SaveChanges();

            var events = new List<Event>
            {
                new Event
                {
                    Id = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                    BookingId = bookings[0].Id,
                    EventName = "Rock Concert",
                    StartTime = DateTimeOffset.Now.AddHours(2).ToUniversalTime(),
                    EndTime = DateTimeOffset.Now.AddHours(5).ToUniversalTime(),
                    TicketsAvailable = 400
                },
                new Event
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                    BookingId = bookings[1].Id,
                    EventName = "Jazz Night",
                    StartTime = DateTimeOffset.Now.AddHours(3).ToUniversalTime(),
                    EndTime = DateTimeOffset.Now.AddHours(6).ToUniversalTime(),
                    TicketsAvailable = 250
                }
            };

            context.Events.AddRange(events);
            context.SaveChanges();
        }
    }
}