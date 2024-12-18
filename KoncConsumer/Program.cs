using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
namespace KoncConsumer;
class Program
{
    private static List<string> announcements = new List<string>();
    private static readonly Guid specificVenueId = Guid.Parse("33333333-3333-3333-3333-333333333333");

    static void Main(string[] args)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            Console.WriteLine("Connecting to RabbitMQ...");

            channel.QueueDeclare(queue: "MyQueue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            Console.WriteLine("Waiting for messages...");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var bookingInfo = JsonSerializer.Deserialize<BookingInfo>(message);

                    if (bookingInfo.VenueId == specificVenueId)
                    {
                        Console.WriteLine($" [x] Received: {message}");
                        announcements.Add(message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");
                }
            };

            channel.BasicConsume(queue: "MyQueue",
                                 autoAck: true,
                                 consumer: consumer);
            while (true)
            {
            }
        } 
    }
}



