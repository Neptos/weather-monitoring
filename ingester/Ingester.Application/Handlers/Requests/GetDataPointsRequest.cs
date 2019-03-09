using Ingester.Application.DataContracts.Requests;
using Ingester.Application.DataContracts.Responses;
using MediatR;

namespace Ingester.Application.Handlers.Requests
{
    public class GetDataPointsRequest : IRequest<DataPointResponse>
    {
        public RequestFilter DataPointsRequestFilter { get; }

        public GetDataPointsRequest(RequestFilter dataPointsRequestFilter)
        {
            DataPointsRequestFilter = dataPointsRequestFilter;
        }
    }
}