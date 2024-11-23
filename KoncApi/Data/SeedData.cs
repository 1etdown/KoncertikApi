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
            // Проверяем, есть ли уже данные в базе
            if (context.Users.Any() || context.Venues.Any() || context.Bookings.Any() || context.Events.Any())
            {
                return;   // База данных уже содержит данные
            }

            // Создание пользователей
            var users = new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "John",
                    Surname = "Doe",
                    PhoneNumber = 1234567890,
                    Email = "john.doe@example.com"
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Jane",
                    Surname = "Smith",
                    PhoneNumber = 9876543210,
                    Email = "jane.smith@example.com"
                }
            };

            // Добавление пользователей в контекст
            context.Users.AddRange(users);
            context.SaveChanges();

            // Создание мест
            var venues = new List<Venue>
            {
                new Venue
                {
                    Id = Guid.NewGuid(),
                    Name = "Concert Hall A",
                    Location = "New York",
                    Capacity = 500
                },
                new Venue
                {
                    Id = Guid.NewGuid(),
                    Name = "Concert Hall B",
                    Location = "Los Angeles",
                    Capacity = 300
                }
            };

            // Добавление мест в контекст
            context.Venues.AddRange(venues);
            context.SaveChanges();

            // Создание бронирований
            var bookings = new List<Booking>
            {
                new Booking
                {
                    Id = Guid.NewGuid(),
                    VenueId = venues[0].Id,
                    UserId = users[0].Id,
                    BookingDate = DateTimeOffset.Now,
                    Status = BookingStatus.Confirmed
                },
                new Booking
                {
                    Id = Guid.NewGuid(),
                    VenueId = venues[1].Id,
                    UserId = users[1].Id,
                    BookingDate = DateTimeOffset.Now,
                    Status = BookingStatus.Pending
                }
            };

            // Добавление бронирований в контекст
            context.Bookings.AddRange(bookings);
            context.SaveChanges();

            // Создание событий
            var events = new List<Event>
            {
                new Event
                {
                    Id = Guid.NewGuid(),
                    BookingId = bookings[0].Id,
                    EventName = "Rock Concert",
                    StartTime = DateTimeOffset.Now.AddHours(2),
                    EndTime = DateTimeOffset.Now.AddHours(5),
                    TicketsAvailable = 400
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    BookingId = bookings[1].Id,
                    EventName = "Jazz Night",
                    StartTime = DateTimeOffset.Now.AddHours(3),
                    EndTime = DateTimeOffset.Now.AddHours(6),
                    TicketsAvailable = 250
                }
            };

            // Добавление событий в контекст
            context.Events.AddRange(events);
            context.SaveChanges();
        }
    }
}
