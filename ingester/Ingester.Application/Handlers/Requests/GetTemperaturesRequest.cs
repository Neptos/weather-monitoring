using Ingester.Application.DataContracts.Requests;
using Ingester.Application.DataContracts.Responses;
using MediatR;

namespace Ingester.Application.Handlers.Requests
{
    public class GetTemperaturesRequest : IRequest<TemperatureResponse>
    {
        public TemperaturesRequestFilter TemperaturesRequestFilter { get; }

        public GetTemperaturesRequest(TemperaturesRequestFilter temperaturesRequestFilter)
        {
            TemperaturesRequestFilter = temperaturesRequestFilter;
        }
    }
}