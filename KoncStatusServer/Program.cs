using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Google.Protobuf.WellKnownTypes;

namespace KoncStatusServer
{
    

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