using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;

namespace KoncStatusConsumer
{
    class Program
    {
        private static readonly string rabbitMqHost = "localhost";
        private static readonly string myQueueName = "MyQueue";
        private static readonly string notifyQueueName = "NotifyQueue";
        private static readonly string grpcServerAddress = "http://localhost:5000";

        static async Task Main(string[] args)
        {
            using var context = new BookingContext();
            context.Database.Migrate();

            if (args.Length > 0 && args[0].ToLower() == "clear")
            {
                await ClearDatabase(context);
                Console.WriteLine("[Consumer] All records cleared from the database.");
                return;
            }
            var factory = new ConnectionFactory() { HostName = rabbitMqHost };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: myQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueDeclare(queue: notifyQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            Console.WriteLine("[Consumer] Connected to RabbitMQ");
            
            using var grpcChannel = GrpcChannel.ForAddress(grpcServerAddress);
            var grpcClient = new BookingStatus.BookingStatusClient(grpcChannel);
            Console.WriteLine("[Consumer] Connected to gRPC Server");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"[RabbitMQ] Received message: {message}");

                try
                {
                    var receivedBooking = JsonSerializer.Deserialize<Booking>(message);
                    Console.WriteLine($"[Consumer] Deserialized booking: ID={receivedBooking.Id}, Status={receivedBooking.Status}");
                    
                    context.Bookings.Add(receivedBooking);
                    await context.SaveChangesAsync();
                    Console.WriteLine($"[Consumer] Booking saved to local DB: ID={receivedBooking.Id}");
                    
                    var localBookingsForDay = context.Bookings
                        .Where(b => b.BookingDate.Date == receivedBooking.BookingDate.Date)
                        .ToList();
                    
                    foreach (var localBooking in localBookingsForDay)
                    {
                        var grpcBookingInfo = new BookingInfoMessage
                        {
                            Id = localBooking.Id,
                            VenueId = localBooking.VenueId,
                            UserId = localBooking.UserId,
                            BookingDate = Timestamp.FromDateTime(localBooking.BookingDate.ToUniversalTime()),
                            Status = localBooking.Status,
                            VenueName = localBooking.VenueName
                        };

                        var updatedBooking = await grpcClient.AddBookingAsync(grpcBookingInfo);
                        Console.WriteLine($"[Consumer] Updated booking status from gRPC: ID={updatedBooking.Id}, Status={updatedBooking.Status}");

                        localBooking.Status = updatedBooking.Status;
                        context.Bookings.Update(localBooking);
                    }
                    
                    if (receivedBooking.Status == "Confirmed")
                    {
                        foreach (var localBooking in localBookingsForDay)
                        {
                            var bookingResponse = await grpcClient.GetBookingByIdAsync(
                                new BookingIdRequest { Id = localBooking.Id }
                            );
                            localBooking.Status = bookingResponse.Status;
                            context.Bookings.Update(localBooking);
                            Console.WriteLine($"[Consumer] Verified updated status: ID={localBooking.Id}, Status={localBooking.Status}");
                        }
                    }

                    await context.SaveChangesAsync();
                    Console.WriteLine($"[Consumer] Local DB updated for date: {receivedBooking.BookingDate.Date}");
                    
                    var notification = new
                    {
                        BookingId = receivedBooking.Id,
                        BookingDate = receivedBooking.BookingDate,
                        Status = receivedBooking.Status,
                        Message = "Booking status changed"
                    };
                    var notificationBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(notification));
                    channel.BasicPublish(exchange: "", routingKey: notifyQueueName, basicProperties: null, body: notificationBody);
                    Console.WriteLine("[Consumer] Sent notification to NotifyQueue.");
                }
                catch (DbUpdateException dbEx)
                {
                    Console.WriteLine($"[Consumer Error] Database update failed: {dbEx.Message}");
                    if (dbEx.InnerException != null)
                        Console.WriteLine($"[Inner Exception] {dbEx.InnerException.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Consumer Error] Message processing failed: {ex.Message}");
                }
            };

            channel.BasicConsume(queue: myQueueName, autoAck: true, consumer: consumer);
            Console.WriteLine("[Consumer] Waiting for messages... Press [Enter] to exit.");
            Console.ReadLine();
        }

        private static async Task ClearDatabase(BookingContext context)
        {
            context.Bookings.RemoveRange(context.Bookings);
            await context.SaveChangesAsync();
        }
    }
   
}