using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ingester.Application.Contracts;
using Ingester.Application.DataContracts.Dtos;
using Ingester.Application.Handlers.Notifications;
using MediatR;

namespace Ingester.Application.Handlers
{
    public class DataPointSavedHandler : INotificationHandler<DataPointSavedNotification>
    {
        private readonly IDataPointPublisher dataPointPublisher;
        private readonly IMapper mapper;

        public DataPointSavedHandler(IDataPointPublisher dataPointPublisher, IMapper mapper)
        {
            this.dataPointPublisher = dataPointPublisher;
            this.mapper = mapper;
        }

        public async Task Handle(DataPointSavedNotification notification, CancellationToken cancellationToken)
        {
            var flatDataPointDto = mapper.Map<FlatDataPointDto>(notification.DataPoint);
            await dataPointPublisher.Publish(flatDataPointDto);
        }
    }
}
