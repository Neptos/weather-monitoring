using System.Collections.Generic;
using Ingester.Application.DataContracts.Dtos;
using MediatR;

namespace Ingester.Application.Queries
{
    public class GetTemperatureListQuery : IRequest<ICollection<TemperatureDto>>
    {
    }
}