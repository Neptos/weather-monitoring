using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Distributor.Application.Contracts;
using Distributor.Application.DataContracts.Dtos;
using Distributor.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Distributor.Infrastructure.RpcClients
{
    public class RpcClient : IRpcClient
    {
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly string replyQueueName;
        private readonly EventingBasicConsumer consumer;
        private readonly BlockingCollection<ICollection<FlatTemperatureDto>> respQueue = new BlockingCollection<ICollection<FlatTemperatureDto>>();
        private readonly IBasicProperties props;

        public RpcClient(IOptionsMonitor<RabbitConfiguration> rabbitConfigurationOptions)
        {
            var factory = new ConnectionFactory() { HostName = rabbitConfigurationOptions.CurrentValue.HostName };

            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            replyQueueName = channel.QueueDeclare().QueueName;
            consumer = new EventingBasicConsumer(channel);

            props = channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;
            props.ReplyTo = replyQueueName;

            consumer.Received += (model, ea) =>
            {
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    var body = ea.Body;
                    var response = Encoding.UTF8.GetString(body);
                    var dtos = JsonConvert.DeserializeObject<ICollection<FlatTemperatureDto>>(response);
                    respQueue.Add(dtos);
                }
            };
        }

        public ICollection<FlatTemperatureDto> FetchCurrentTemperatures()
        {
            var messageBytes = Encoding.UTF8.GetBytes("fetchCurrentTemperatures");
            channel.BasicPublish(
                exchange: "",
                routingKey: "rpc_queue",
                basicProperties: props,
                body: messageBytes);

            channel.BasicConsume(
                consumer: consumer,
                queue: replyQueueName,
                autoAck: true);

            return respQueue.Take();
        }
    }
}