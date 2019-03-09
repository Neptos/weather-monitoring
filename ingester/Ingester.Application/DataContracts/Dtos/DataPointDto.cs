using System;

namespace Ingester.Application.DataContracts.Dtos
{
    public class DataPointDto
    {
        public string Id { get; set; }
        public float Value { get; set; }
        public DateTime Timestamp { get; set; }
        public string Type { get; set; }
    }
}