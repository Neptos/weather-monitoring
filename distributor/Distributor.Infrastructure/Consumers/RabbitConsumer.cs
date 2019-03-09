using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Distributor.Application.Contracts;
using Distributor.Application.DataContracts.Dtos;
using Distributor.Infrastructure.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Distributor.Infrastructure.Consumers
{
    public class RabbitConsumer : IHostedService
    {
        private const string Exchange = "weather";
        private readonly RabbitConfiguration rabbitConfiguration;
        private readonly IServiceProvider serviceProvider;
        private Task worker;

        public RabbitConsumer(IOptionsMonitor<RabbitConfiguration> rabbitConfigurationOptions, IServiceProvider serviceProvider)
        {
            this.rabbitConfiguration = rabbitConfigurationOptions.CurrentValue;
            this.serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            worker = Task.Run(() =>
            {
                var factory = new ConnectionFactory() { HostName = rabbitConfiguration.HostName };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: Exchange, type: "topic");

                    var queueName = channel.QueueDeclare().QueueName;
                    channel.QueueBind(queue: queueName,
                                      exchange: Exchange,
                                      routingKey: "#");

                    Console.WriteLine("Starting consumer");

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += async (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        var dto = JsonConvert.DeserializeObject<FlatDataPointDto>(message);
                        using (var serviceScope = serviceProvider.CreateScope())
                        {
                            var dataPointService = serviceScope.ServiceProvider.GetRequiredService<IDataPointService>();
                            await dataPointService.NewDataPointReceivedAsync(dto);
                        }
                    };
                    channel.BasicConsume(queue: queueName,
                                         autoAck: true,
                                         consumer: consumer);
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        Thread.Sleep(1000);
                    }
                    Console.WriteLine("Stopping consumer");
                }
            });
            return worker;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.FromCanceled(cancellationToken);
        }
    }
}