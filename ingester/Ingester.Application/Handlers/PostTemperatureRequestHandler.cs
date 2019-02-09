using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ingester.Application.Handlers.Requests;
using Ingester.Domain.Models;
using Ingester.Persistence;
using MediatR;
using Ingester.Application.Handlers.Notifications;
using Microsoft.EntityFrameworkCore;

namespace Ingester.Application.Handlers
{
    public class PostTemperatureRequestHandler : IRequestHandler<PostTemperatureRequest>
    {
        private readonly WeatherDbContext context;
        private readonly IMapper mapper;
        private readonly IMediator mediator;

        public PostTemperatureRequestHandler(WeatherDbContext context, IMapper mapper, IMediator mediator)
        {
            this.context = context;
            this.mapper = mapper;
            this.mediator = mediator;
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

            var fetchedTemperature = context.Temperatures
                .Include(t => t.Sensor)
                    .ThenInclude(s => s.Location)
                .FirstOrDefault(t => t.Id == temperature.Id);

            await mediator.Publish(new TemperatureSavedNotification(fetchedTemperature));
            return Unit.Value;
        }
    }
}
