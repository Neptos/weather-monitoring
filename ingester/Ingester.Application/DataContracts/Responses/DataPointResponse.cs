using System.Collections.Generic;
using Ingester.Application.DataContracts.Dtos;

namespace Ingester.Application.DataContracts.Responses
{
    public class DataPointResponse
    {
        public ICollection<FlatDataPointDto> DataPoints { get; set; }
    }
}