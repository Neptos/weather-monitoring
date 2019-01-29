using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ingester.Application.Commands;
using Ingester.Application.DataContracts.Requests;
using Ingester.Domain.Models;
using Ingester.Persistence;
using MediatR;

namespace Ingester.Application.Handlers
{
    public class PutTemperatureCommandHandler : IRequestHandler<PutTemperatureCommand, bool>
    {
        private readonly TemperatureDbContext context;
        private readonly IMapper mapper;

        public PutTemperatureCommandHandler(TemperatureDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<bool> Handle(PutTemperatureCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                var temperature = context.Temperatures.FirstOrDefault(temp => temp.Id == request.Id);
                if (temperature == null)
                {
                    return false;
                }
                mapper.Map<TemperatureRequest, Temperature>(request.TemperatureRequest, temperature);
                context.Temperatures.Update(temperature);
                await context.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }
    }
}