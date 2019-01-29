using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ingester.Application.DataContracts.Dtos;
using Ingester.Application.Queries;
using Ingester.Persistence;
using MediatR;

namespace Ingester.Application.Handlers
{
    public class GetTemperatureListQueryHandler : IRequestHandler<GetTemperatureListQuery, ICollection<TemperatureDto>>
    {
        private readonly TemperatureDbContext context;
        private readonly IMapper mapper;

        public GetTemperatureListQueryHandler(TemperatureDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<ICollection<TemperatureDto>> Handle(GetTemperatureListQuery request, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                return mapper.Map<ICollection<TemperatureDto>>(context.Temperatures.ToList());
            });
        }
    }
}
