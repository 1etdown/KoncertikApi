using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Google.Protobuf.WellKnownTypes;

namespace KoncStatusServer
{
    public class BookingStatusService : BookingStatus.BookingStatusBase
    {
        private readonly List<BookingInfoMessage> bookings = new();

        public override Task<BookingInfoMessage> AddBooking(BookingInfoMessage request, ServerCallContext context)
        {
            Console.WriteLine($"[gRPC] Processing booking: ID={request.Id}, Status={request.Status}");

            var bookingDate = request.BookingDate.ToDateTime().Date;

            if (request.Status == "Confirmed")
            {
                foreach (var booking in bookings.Where(b => b.BookingDate.ToDateTime().Date == bookingDate && b.Status != "Confirmed"))
                {
                    booking.Status = "Canceled";
                    Console.WriteLine($"[Server] Booking {booking.Id} set to 'Canceled'");
                }
            }

            var existingBooking = bookings.FirstOrDefault(b => b.Id == request.Id);
            if (existingBooking != null)
            {
                bookings.Remove(existingBooking);
                Console.WriteLine($"[Server] Removed existing booking with ID={existingBooking.Id}");
            }

            bookings.Add(request);
            Console.WriteLine($"[Server] Added/Updated booking: ID={request.Id}, Status={request.Status}");
            return Task.FromResult(request);
        }

        public override Task<BookingInfoMessage> GetBookingById(BookingIdRequest request, ServerCallContext context)
        {
            var booking = bookings.FirstOrDefault(b => b.Id == request.Id);
            if (booking == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Booking with ID={request.Id} not found"));
            }

            return Task.FromResult(booking);
        }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            const string host = "localhost";
            const int port = 5000;

            var server = new Server
            {
                Services = { BookingStatus.BindService(new BookingStatusService()) },
                Ports = { new ServerPort(host, port, ServerCredentials.Insecure) }
            };

            server.Start();

            Console.WriteLine($"[Server] Listening on {host}:{port}");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }
    }
}
