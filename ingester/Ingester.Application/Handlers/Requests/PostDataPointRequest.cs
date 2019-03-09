using Ingester.Application.DataContracts.Requests;
using MediatR;

namespace Ingester.Application.Handlers.Requests
{
    public class PostDataPointRequest : IRequest
    {
        public DataPointRequest DataPointRequest { get; }

        public PostDataPointRequest(DataPointRequest dataPointRequest)
        {
            DataPointRequest = dataPointRequest;
        }
    }
}