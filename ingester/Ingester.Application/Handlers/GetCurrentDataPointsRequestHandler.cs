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
    public class GetCurrentDataPointsRequestHandler : IRequestHandler<GetCurrentDataPointsRequest, DataPointResponse>
    {
        private readonly WeatherDbContext context;
        private readonly IMapper mapper;

        public GetCurrentDataPointsRequestHandler(WeatherDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<DataPointResponse> Handle(GetCurrentDataPointsRequest request, CancellationToken cancellationToken)
        {
            ICollection<DataPoint> dataPoints = new List<DataPoint>();

            foreach (var sensor in context.Sensors)
            {
                var temperature = await context.DataPoints
                    .Include(t => t.Sensor)
                        .ThenInclude(s => s.Location)
                    .Where(t => t.SensorId == sensor.Id
                        && t.Type == "Temperature")
                    .OrderByDescending(t => t.Timestamp)
                    .FirstOrDefaultAsync();
                var dewPoint = await context.DataPoints
                    .Include(t => t.Sensor)
                        .ThenInclude(s => s.Location)
                    .Where(t => t.SensorId == sensor.Id
                        && t.Type == "DewPoint")
                    .OrderByDescending(t => t.Timestamp)
                    .FirstOrDefaultAsync();
                var precipitation = await context.DataPoints
                    .Include(t => t.Sensor)
                        .ThenInclude(s => s.Location)
                    .Where(t => t.SensorId == sensor.Id
                        && t.Type == "Precipitation")
                    .OrderByDescending(t => t.Timestamp)
                    .FirstOrDefaultAsync();
                var pressure = await context.DataPoints
                    .Include(t => t.Sensor)
                        .ThenInclude(s => s.Location)
                    .Where(t => t.SensorId == sensor.Id
                        && t.Type == "Pressure")
                    .OrderByDescending(t => t.Timestamp)
                    .FirstOrDefaultAsync();
                var relativeHumidity = await context.DataPoints
                    .Include(t => t.Sensor)
                        .ThenInclude(s => s.Location)
                    .Where(t => t.SensorId == sensor.Id
                        && t.Type == "RelativeHumidity")
                    .OrderByDescending(t => t.Timestamp)
                    .FirstOrDefaultAsync();
                var visibility = await context.DataPoints
                    .Include(t => t.Sensor)
                        .ThenInclude(s => s.Location)
                    .Where(t => t.SensorId == sensor.Id
                        && t.Type == "Visibility")
                    .OrderByDescending(t => t.Timestamp)
                    .FirstOrDefaultAsync();
                var windDirection = await context.DataPoints
                    .Include(t => t.Sensor)
                        .ThenInclude(s => s.Location)
                    .Where(t => t.SensorId == sensor.Id
                        && t.Type == "WindDirection")
                    .OrderByDescending(t => t.Timestamp)
                    .FirstOrDefaultAsync();
                var windGust = await context.DataPoints
                    .Include(t => t.Sensor)
                        .ThenInclude(s => s.Location)
                    .Where(t => t.SensorId == sensor.Id
                        && t.Type == "WindGust")
                    .OrderByDescending(t => t.Timestamp)
                    .FirstOrDefaultAsync();
                var windSpeed = await context.DataPoints
                    .Include(t => t.Sensor)
                        .ThenInclude(s => s.Location)
                    .Where(t => t.SensorId == sensor.Id
                        && t.Type == "WindSpeed")
                    .OrderByDescending(t => t.Timestamp)
                    .FirstOrDefaultAsync();
                dataPoints.Add(temperature);
                dataPoints.Add(dewPoint);
                dataPoints.Add(precipitation);
                dataPoints.Add(pressure);
                dataPoints.Add(relativeHumidity);
                dataPoints.Add(visibility);
                dataPoints.Add(windDirection);
                dataPoints.Add(windGust);
                dataPoints.Add(windSpeed);
            }

            var response = new DataPointResponse
            {
                DataPoints = mapper.Map<ICollection<FlatDataPointDto>>(dataPoints)
            };
            return response;
        }
    }
}
