using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Planning_Service.Models;

namespace ServiceWorker
{
    public class Worker : BackgroundService
    {
        private readonly string _csvFilePath = "people.csv";
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "deliveryQueue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var delivery = new EventingBasicConsumer(channel);
            delivery.Received += async (model, ea) =>
            {
                var shipment = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(shipment);
                Console.WriteLine($" [x] Received {message}");

                var parts = message.Split(',');
                var name = parts[0];
                var city = parts[1];

                if (!File.Exists(_csvFilePath))
                {
                    using (var writer = new StreamWriter(_csvFilePath, append: true))
                    {
                        await writer.WriteLineAsync("Name,City");
                    }
                }

                using (var writer = new StreamWriter(_csvFilePath, append: true))
                {
                    await writer.WriteLineAsync($"{name},{city}");
                }
            };

            channel.BasicConsume(queue: "shipmentQueue",
                                 autoAck: true,
                                 consumer: consumer);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
