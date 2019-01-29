using Ingester.Application.DataContracts.Requests;
using MediatR;

namespace Ingester.Application.Commands
{
    public class PutTemperatureCommand : IRequest<bool>
    {
        public TemperatureRequest TemperatureRequest { get; }
        public string Id { get; }

        public PutTemperatureCommand(string id, TemperatureRequest temperatureRequest)
        {
            Id = id;
            TemperatureRequest = temperatureRequest;
        }
    }
}