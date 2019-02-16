using System.Collections.Generic;
using Ingester.Application.DataContracts.Dtos;

namespace Ingester.Application.DataContracts.Responses
{
    public class TemperatureResponse
    {
        public ICollection<FlatTemperatureDto> Temperatures { get; set; }
    }
}