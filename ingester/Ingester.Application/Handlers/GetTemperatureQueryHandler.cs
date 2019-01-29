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
    public class GetTemperatureQueryHandler : IRequestHandler<GetTemperatureQuery, TemperatureDto>
    {
        private readonly TemperatureDbContext context;
        private readonly IMapper mapper;

        public GetTemperatureQueryHandler(TemperatureDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<TemperatureDto> Handle(GetTemperatureQuery request, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                return mapper.Map<TemperatureDto>(context.Temperatures.FirstOrDefault(temperature => temperature.Id == request.Id));
            });
        }
    }
}