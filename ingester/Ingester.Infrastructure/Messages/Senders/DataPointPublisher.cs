using System.Text;
using System.Threading.Tasks;
using Ingester.Application.Contracts;
using Ingester.Application.DataContracts.Dtos;
using Ingester.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Ingester.Infrastructure.Messages.Senders
{
    public class DataPointPublisher : IDataPointPublisher
    {
        private readonly RabbitConfiguration rabbitConfiguration;

        public DataPointPublisher(IOptionsMonitor<RabbitConfiguration> rabbitConfigurationOptions)
        {
            this.rabbitConfiguration = rabbitConfigurationOptions.CurrentValue;
        }

        public Task Publish(FlatDataPointDto flatDataPointDto)
        {
            var factory = new ConnectionFactory() { HostName = rabbitConfiguration.HostName };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var exchange = "weather";
                var routingKey = $"{flatDataPointDto.LocationName}.{flatDataPointDto.SensorId}";
                var message = JsonConvert.SerializeObject(flatDataPointDto);
                var body = Encoding.UTF8.GetBytes(message);

                channel.ExchangeDeclare(exchange: exchange,
                                    type: "topic");

                channel.BasicPublish(exchange: exchange,
                                     routingKey: routingKey,
                                     basicProperties: null,
                                     body: body);
            }
            return Task.CompletedTask;
        }
    }
}