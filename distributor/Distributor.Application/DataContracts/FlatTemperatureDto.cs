using System;

namespace Distributor.Application.DataContracts.Dtos
{
    public class FlatTemperatureDto
    {
        public string Id { get; set; }
        public float Value { get; set; }
        public DateTime Timestamp { get; set; }
        public string SensorId { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
    }
}