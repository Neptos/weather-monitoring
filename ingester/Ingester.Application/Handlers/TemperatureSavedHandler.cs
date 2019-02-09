using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ingester.Application.Contracts;
using Ingester.Application.DataContracts.Dtos;
using Ingester.Application.Handlers.Notifications;
using MediatR;

namespace Ingester.Application.Handlers
{
    public class TemperatureSavedHandler : INotificationHandler<TemperatureSavedNotification>
    {
        private readonly ITemperaturePublisher temperaturePublisher;
        private readonly IMapper mapper;

        public TemperatureSavedHandler(ITemperaturePublisher temperaturePublisher, IMapper mapper)
        {
            this.temperaturePublisher = temperaturePublisher;
            this.mapper = mapper;
        }

        public async Task Handle(TemperatureSavedNotification notification, CancellationToken cancellationToken)
        {
            var flatTemperatureDto = mapper.Map<FlatTemperatureDto>(notification.Temperature);
            await temperaturePublisher.Publish(flatTemperatureDto);
        }
    }
}
