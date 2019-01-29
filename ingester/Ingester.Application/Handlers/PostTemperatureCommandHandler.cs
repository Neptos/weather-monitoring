using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ingester.Application.Commands;
using Ingester.Domain.Models;
using Ingester.Persistence;
using MediatR;

namespace Ingester.Application.Handlers
{
    public class PostTemperatureCommandHandler : IRequestHandler<PostTemperatureCommand, string>
    {
        private readonly TemperatureDbContext context;
        private readonly IMapper mapper;

        public PostTemperatureCommandHandler(TemperatureDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<string> Handle(PostTemperatureCommand request, CancellationToken cancellationToken)
        {
            var temperature = mapper.Map<Temperature>(request.TemperatureRequest);
            temperature.Id = Guid.NewGuid().ToString();
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                await context.Temperatures.AddAsync(temperature);
                await context.SaveChangesAsync();
                transaction.Commit();
            }
            return temperature.Id;
        }
    }
}
