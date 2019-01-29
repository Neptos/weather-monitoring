using System;

namespace Ingester.Application.DataContracts.Requests
{
    public class TemperatureRequest
    {
        public float Value { get; set; }
        public DateTime Timestamp { get; set; }
    }
}