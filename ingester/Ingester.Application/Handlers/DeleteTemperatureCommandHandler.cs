using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ingester.Application.Commands;
using Ingester.Persistence;
using MediatR;

namespace Ingester.Application.Handlers
{
    public class DeleteTemperatureCommandHandler : IRequestHandler<DeleteTemperatureCommand, bool>
    {
        private readonly TemperatureDbContext context;

        public DeleteTemperatureCommandHandler(TemperatureDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> Handle(DeleteTemperatureCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                var temperature = context.Temperatures.FirstOrDefault(temp => temp.Id == request.Id);
                if (temperature == null)
                {
                    return false;
                }
                context.Temperatures.Remove(temperature);
                await context.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }
    }
}