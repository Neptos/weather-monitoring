using Ingester.Application.DataContracts.Requests;
using Ingester.Application.DataContracts.Responses;
using MediatR;

namespace Ingester.Application.Handlers.Requests
{
    public class GetCurrentDataPointsRequest : IRequest<DataPointResponse>
    {
    }
}