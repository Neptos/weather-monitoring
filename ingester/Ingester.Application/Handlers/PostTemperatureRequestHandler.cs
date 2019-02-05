using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ingester.Application.Handlers.Requests;
using Ingester.Domain.Models;
using Ingester.Persistence;
using MediatR;

namespace Ingester.Application.Handlers
{
    public class PostTemperatureRequestHandler : IRequestHandler<PostTemperatureRequest>
    {
        private readonly WeatherDbContext context;
        private readonly IMapper mapper;

        public PostTemperatureRequestHandler(WeatherDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Unit> Handle(PostTemperatureRequest request, CancellationToken cancellationToken)
        {
            var location = context.Locations.FirstOrDefault(l => l.Name == request.TemperatureRequest.Location);

            if (location == null)
            {
                location = new Location
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = request.TemperatureRequest.Location
                };
                await context.Locations.AddAsync(location);
            }

            var sensor = await context.Sensors.FindAsync(request.TemperatureRequest.SensorId);
            if (sensor == null)
            {
                sensor = new Sensor
                {
                    Id = request.TemperatureRequest.SensorId,
                    LocationId = location.Id
                };
                await context.Sensors.AddAsync(sensor);
            }

            var temperature = mapper.Map<Temperature>(request.TemperatureRequest);
            temperature.Id = Guid.NewGuid().ToString();
            temperature.SensorId = sensor.Id;
            await context.Temperatures.AddAsync(temperature);

            await context.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
