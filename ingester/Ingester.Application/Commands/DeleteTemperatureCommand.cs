using MediatR;

namespace Ingester.Application.Commands
{
    public class DeleteTemperatureCommand : IRequest<bool>
    {
        public string Id { get; }

        public DeleteTemperatureCommand(string id)
        {
            Id = id;
        }
    }
}