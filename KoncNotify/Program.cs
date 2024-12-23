using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NotifyService
{
    public class NotificationMessage
    {
        public string BookingId { get; set; }
        public DateTime BookingDate { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }

    class Program
    {
        static async Task Main()
        {
            Console.WriteLine("[Notifier] Starting...");
            
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            const string notifyQueueName = "NotifyQueue";
            channel.QueueDeclare(
                queue: notifyQueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            Console.WriteLine("[Notifier] Connected to RabbitMQ, listening on NotifyQueue.");

            var hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7123/bookinghub")
                .Build();

            await hubConnection.StartAsync();
            Console.WriteLine("[Notifier] Connected to KoncAPI’s SignalR Hub.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, args) =>
            {
                var body = args.Body.ToArray();
                var rawJson = Encoding.UTF8.GetString(body);
                Console.WriteLine($"[Notifier] Received message: {rawJson}");

                try
                {
                    var notification = JsonSerializer.Deserialize<NotificationMessage>(rawJson);
                    
                    if (notification != null && 
                        !string.IsNullOrEmpty(notification.BookingId) &&
                        !string.IsNullOrEmpty(notification.Status))
                    {
                        await hubConnection.InvokeAsync(
                            "SendBookingStatus",
                            notification.BookingId,
                            notification.Status
                        );

                        Console.WriteLine($"[Notifier] Sent status update via SignalR: {notification.BookingId} -> {notification.Status}");
                    }
                    else
                    {
                        Console.WriteLine("[Notifier] Invalid or incomplete message format.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Notifier] Error parsing/forwarding message: {ex.Message}");
                }
            };

            channel.BasicConsume(queue: notifyQueueName, autoAck: true, consumer: consumer);
            Console.WriteLine("[Notifier] Waiting for messages... Press [Enter] to exit.");
            Console.ReadLine();
        }
    }
}