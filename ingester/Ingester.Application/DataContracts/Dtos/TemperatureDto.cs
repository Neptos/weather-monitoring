using System;

namespace Ingester.Application.DataContracts.Dtos
{
    public class TemperatureDto
    {
        public string Id { get; set; }
        public float Value { get; set; }
        public DateTime Timestamp { get; set; }
    }
}