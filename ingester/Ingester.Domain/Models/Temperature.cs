using System;

namespace Ingester.Domain.Models
{
    public class Temperature
    {
        public string Id { get; set; }
        public float Value { get; set; }
        public DateTime Timestamp { get; set; }
    }
}