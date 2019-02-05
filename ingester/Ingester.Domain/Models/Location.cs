using System;
using System.Collections.Generic;

namespace Ingester.Domain.Models
{
    public class Location
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<Sensor> Sensors { get; set; }
    }
}
