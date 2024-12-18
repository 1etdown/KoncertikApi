using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
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
        private static readonly string queueName = "MyQueue";
        private static readonly string grpcServerAddress = "http://localhost:5000";

        static async Task Main(string[] args)
        {
            using var context = new BookingContext();
            context.Database.Migrate();

            var factory = new ConnectionFactory() { HostName = rabbitMqHost };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            Console.WriteLine("Connected to RabbitMQ");

            using var grpcChannel = GrpcChannel.ForAddress(grpcServerAddress);
            var grpcClient = new BookingStatus.BookingStatusClient(grpcChannel);
            Console.WriteLine("Connected to gRPC Server");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"[RabbitMQ] Received: {message}");

                try
                {
                    // Deserialize and save to local DB
                    var bookingInfo = JsonSerializer.Deserialize<Booking>(message);
                    context.Bookings.Add(bookingInfo);
                    await context.SaveChangesAsync();
                    Console.WriteLine($"[Database] Booking saved locally: ID={bookingInfo.Id}");

                    // Send to gRPC Server
                    var grpcBookingInfo = new BookingInfoMessage
                    {
                        Id = bookingInfo.Id,
                        VenueId = bookingInfo.VenueId,
                        UserId = bookingInfo.UserId,
                        BookingDate = Timestamp.FromDateTime(bookingInfo.BookingDate.ToUniversalTime()),
                        Status = bookingInfo.Status,
                        VenueName = bookingInfo.VenueName
                    };

                    var updatedBooking = await grpcClient.AddBookingAsync(grpcBookingInfo);

                    // Get updated status from gRPC
                    var newStatus = updatedBooking.Status;

                    // Update local DB with new status
                    bookingInfo.Status = newStatus;
                    context.Bookings.Update(bookingInfo);
                    await context.SaveChangesAsync();
                    Console.WriteLine($"[Database] Booking updated locally: ID={bookingInfo.Id}, Status={bookingInfo.Status}");
                }
                catch (DbUpdateException dbEx)
                {
                    Console.WriteLine($"[Error] Database update failed: {dbEx.Message}");
                    if (dbEx.InnerException != null)
                    {
                        Console.WriteLine($"[Inner Exception] {dbEx.InnerException.Message}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Error] Failed to process message: {ex.Message}");
                }
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            Console.WriteLine("Waiting for messages... Press [Enter] to exit.");
            Console.ReadLine();
        }
        
    }

    public class Booking
    {
        private DateTime _bookingDate;
        public string Id { get; set; }
        public string VenueId { get; set; }
        public string UserId { get; set; }
        public DateTime BookingDate   {
            get => _bookingDate;
            set => _bookingDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }
        public string Status { get; set; }
        public string VenueName { get; set; }
    }

    public class BookingContext : DbContext
    {
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5444;Database=statusdb;Username=pguser;Password=0000");

        }
        
    }
}

