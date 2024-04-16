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
using System.Text.Json;
using Planning_Service.Services;

namespace Planning_Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private DeliveryService _deliveryService;

        public Worker(ILogger<Worker> logger, DeliveryService serivce)
        {
            _logger = logger;
            _deliveryService = serivce;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory { HostName = Environment.GetEnvironmentVariable("RabbitMQHost") };

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
                var uftString = Encoding.UTF8.GetString(shipment);
                var message = JsonSerializer.Deserialize<Delivery>(uftString);
                Console.WriteLine($" [x] Received {message}");

                await _deliveryService.CreateAsync(message);
            };

            channel.BasicConsume(queue: "deliveryQueue",
                                 autoAck: true,
                                 consumer: delivery);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
