using Ingester.Application.DataContracts.Dtos;
using MediatR;

namespace Ingester.Application.Queries
{
    public class GetTemperatureQuery : IRequest<TemperatureDto>
    {
        public string Id { get; }

        public GetTemperatureQuery(string id)
        {
            Id = id;
        }
    }
}