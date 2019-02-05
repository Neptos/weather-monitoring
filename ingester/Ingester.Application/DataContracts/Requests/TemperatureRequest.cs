using System;

namespace Ingester.Application.DataContracts.Requests
{
    public class TemperatureRequest
    {
        public float Value { get; set; }
        public DateTime Timestamp { get; set; }
        public string SensorId { get; set; }
        public string Location { get; set; }
    }
}