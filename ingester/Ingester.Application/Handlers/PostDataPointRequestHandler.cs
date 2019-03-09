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
    public class PostDataPointRequestHandler : IRequestHandler<PostDataPointRequest>
    {
        private readonly WeatherDbContext context;
        private readonly IMapper mapper;
        private readonly IMediator mediator;

        public PostDataPointRequestHandler(WeatherDbContext context, IMapper mapper, IMediator mediator)
        {
            this.context = context;
            this.mapper = mapper;
            this.mediator = mediator;
        }

        public async Task<Unit> Handle(PostDataPointRequest request, CancellationToken cancellationToken)
        {
            var location = context.Locations.FirstOrDefault(l => l.Name == request.DataPointRequest.Location);

            if (location == null)
            {
                location = new Location
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = request.DataPointRequest.Location
                };
                await context.Locations.AddAsync(location);
            }

            var sensor = await context.Sensors.FindAsync(request.DataPointRequest.SensorId);
            if (sensor == null)
            {
                sensor = new Sensor
                {
                    Id = request.DataPointRequest.SensorId,
                    LocationId = location.Id
                };
                await context.Sensors.AddAsync(sensor);
            }

            var dataPoint = mapper.Map<DataPoint>(request.DataPointRequest);
            dataPoint.Id = Guid.NewGuid().ToString();
            dataPoint.SensorId = sensor.Id;
            await context.DataPoints.AddAsync(dataPoint);

            await context.SaveChangesAsync();

            var fetchedDataPoint = context.DataPoints
                .Include(t => t.Sensor)
                    .ThenInclude(s => s.Location)
                .FirstOrDefault(t => t.Id == dataPoint.Id);

            await mediator.Publish(new DataPointSavedNotification(fetchedDataPoint));
            return Unit.Value;
        }
    }
}
