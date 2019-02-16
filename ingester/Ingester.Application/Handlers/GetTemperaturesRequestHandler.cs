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
    public class GetTemperaturesRequestHandler : IRequestHandler<GetTemperaturesRequest, TemperatureResponse>
    {
        private readonly WeatherDbContext context;
        private readonly IMapper mapper;

        public GetTemperaturesRequestHandler(WeatherDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<TemperatureResponse> Handle(GetTemperaturesRequest request, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
             {
                 var temperatures = context.Temperatures
                     .Include(t => t.Sensor)
                         .ThenInclude(s => s.Location)
                     .Where(t => t.Timestamp >= request.TemperaturesRequestFilter.From
                         && t.Timestamp <= request.TemperaturesRequestFilter.To
                         && t.Sensor.Location.Name == request.TemperaturesRequestFilter.Location).ToList();

                 var response = new TemperatureResponse
                 {
                     Temperatures = mapper.Map<ICollection<FlatTemperatureDto>>(temperatures)
                 };
                 return response;
             });
        }
    }
}
