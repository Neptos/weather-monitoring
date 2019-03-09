using System;

namespace Ingester.Domain.Models
{
    public class DataPoint
    {
        public string Id { get; set; }
        public float Value { get; set; }
        public DateTime Timestamp { get; set; }
        public string SensorId { get; set; }
        public Sensor Sensor { get; set; }
        public string Type { get; set; }
    }
}
