using Ingester.Application.DataContracts.Requests;
using MediatR;

namespace Ingester.Application.Handlers.Requests
{
    public class PostTemperatureRequest : IRequest
    {
        public TemperatureRequest TemperatureRequest { get; }

        public PostTemperatureRequest(TemperatureRequest temperatureRequest)
        {
            TemperatureRequest = temperatureRequest;
        }
    }
}