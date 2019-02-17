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
using Ingester.Domain.Models;

namespace Ingester.Application.Handlers
{
    public class GetCurrentTemperatureRequestHandler : IRequestHandler<GetCurrentTemperatureRequest, TemperatureResponse>
    {
        private readonly WeatherDbContext context;
        private readonly IMapper mapper;

        public GetCurrentTemperatureRequestHandler(WeatherDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<TemperatureResponse> Handle(GetCurrentTemperatureRequest request, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                ICollection<Temperature> temperatures = new List<Temperature>();

                foreach (var sensor in context.Sensors)
                {
                    var temperature = context.Temperatures
                        .Include(t => t.Sensor)
                            .ThenInclude(s => s.Location)
                        .Where(t => t.SensorId == sensor.Id)
                        .OrderByDescending(t => t.Timestamp)
                        .First();
                    temperatures.Add(temperature);
                }

                var response = new TemperatureResponse
                {
                    Temperatures = mapper.Map<ICollection<FlatTemperatureDto>>(temperatures)
                };
                return response;
            });
        }
    }
}
