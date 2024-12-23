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
            Console.WriteLine($"[gRPC Server] Received AddBooking request: ID={request.Id}, Status={request.Status}");
            try
            {
                var bookingDate = request.BookingDate.ToDateTime().Date;

                // Если новый запрос Confirmed, отменяем все не Confirmed бронирования на ту же дату
                if (request.Status == "Confirmed")
                {
                    foreach (var booking in bookings.Where(b => b.BookingDate.ToDateTime().Date == bookingDate))
                    {
                        if (booking.Status != "Confirmed")
                        {
                            booking.Status = "Canceled";
                            Console.WriteLine($"[gRPC Server] Booking {booking.Id} set to 'Canceled'");
                        }
                    }
                }

                // Удаляем дубликат, если он уже есть в списке
                var existingBooking = bookings.FirstOrDefault(b => b.Id == request.Id);
                if (existingBooking != null)
                {
                    bookings.Remove(existingBooking);
                    Console.WriteLine($"[gRPC Server] Removed existing booking with ID={existingBooking.Id}");
                }

                // Добавляем новое или обновлённое бронирование
                bookings.Add(request);
                Console.WriteLine($"[gRPC Server] Added/Updated booking: ID={request.Id}, Status={request.Status}");

                // Находим реальный объект в списке с актуальным статусом и возвращаем его
                var updatedBooking = bookings.First(b => b.Id == request.Id);
                return Task.FromResult(updatedBooking);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[gRPC Server] Error in AddBooking: {ex.Message}");
                throw;
            }
        }

        public override Task<BookingInfoMessage> GetBookingById(BookingIdRequest request, ServerCallContext context)
        {
            Console.WriteLine($"[gRPC Server] Received GetBookingById request: ID={request.Id}");
            try
            {
                var booking = bookings.FirstOrDefault(b => b.Id == request.Id);
                if (booking == null)
                {
                    Console.WriteLine($"[gRPC Server] Booking with ID={request.Id} not found");
                    throw new RpcException(new Status(StatusCode.NotFound, $"Booking with ID={request.Id} not found"));
                }

                return Task.FromResult(booking);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[gRPC Server] Error in GetBookingById: {ex.Message}");
                throw;
            }
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

            Console.WriteLine($"[gRPC Server] Listening on {host}:{port}");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }
    }
}