using Ingester.Application.DataContracts.Requests;
using MediatR;

namespace Ingester.Application.Commands
{
    public class PostTemperatureCommand : IRequest<string>
    {
        public TemperatureRequest TemperatureRequest { get; }

        public PostTemperatureCommand(TemperatureRequest temperatureRequest)
        {
            TemperatureRequest = temperatureRequest;
        }
    }
}