using System;
using System.Collections.Generic;

namespace Ingester.Domain.Models
{
    public class Sensor
    {
        public string Id { get; set; }
        public ICollection<Temperature> Temperatures { get; set; }
        public string LocationId { get; set; }
        public Location Location { get; set; }
    }
}
