using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ingester.Application.Handlers.Requests;
using Ingester.Persistence;
using MediatR;
using Ingester.Application.DataContracts.Responses;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Ingester.Application.DataContracts.Dtos;

namespace Ingester.Application.Handlers
{
    public class GetDataPointsRequestHandler : IRequestHandler<GetDataPointsRequest, DataPointResponse>
    {
        private readonly WeatherDbContext context;
        private readonly IMapper mapper;

        public GetDataPointsRequestHandler(WeatherDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<DataPointResponse> Handle(GetDataPointsRequest request, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
             {
                 var dataPoints = context.DataPoints
                     .Include(t => t.Sensor)
                         .ThenInclude(s => s.Location)
                     .Where(t => t.Timestamp >= request.DataPointsRequestFilter.From
                         && t.Timestamp <= request.DataPointsRequestFilter.To
                         && t.Sensor.Location.Name == request.DataPointsRequestFilter.Location).ToList();

                 var response = new DataPointResponse
                 {
                     DataPoints = mapper.Map<ICollection<FlatDataPointDto>>(dataPoints)
                 };
                 return response;
             });
        }
    }
}
