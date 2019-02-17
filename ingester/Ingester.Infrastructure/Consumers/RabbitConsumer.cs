using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ingester.Application.Handlers.Requests;
using Ingester.Infrastructure.Configurations;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Ingester.Infrastructure.Consumers
{
    public class RabbitConsumer : IHostedService
    {
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
                    channel.QueueDeclare(queue: "rpc_queue", durable: false,
                      exclusive: false, autoDelete: false, arguments: null);
                    channel.BasicQos(0, 1, false);
                    var consumer = new EventingBasicConsumer(channel);
                    channel.BasicConsume(queue: "rpc_queue",
                      autoAck: false, consumer: consumer);
                    Console.WriteLine(" [x] Awaiting RPC requests");

                    consumer.Received += async (model, ea) =>
                    {
                        string response = null;

                        var props = ea.BasicProperties;
                        var replyProps = channel.CreateBasicProperties();
                        replyProps.CorrelationId = props.CorrelationId;

                        try
                        {
                            using (var serviceScope = serviceProvider.CreateScope())
                            {
                                var mediator = serviceScope.ServiceProvider.GetRequiredService<IMediator>();
                                var request = new GetCurrentTemperatureRequest();
                                var temperatures = await mediator.Send(request);
                                response = JsonConvert.SerializeObject(temperatures.Temperatures);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(" [.] " + e.Message);
                            response = "";
                        }
                        finally
                        {
                            var responseBytes = Encoding.UTF8.GetBytes(response);
                            channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                              basicProperties: replyProps, body: responseBytes);
                            channel.BasicAck(deliveryTag: ea.DeliveryTag,
                              multiple: false);
                        }
                    };

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