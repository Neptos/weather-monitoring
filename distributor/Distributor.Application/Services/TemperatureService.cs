using System.Threading.Tasks;
using Distributor.Application.Contracts;
using Distributor.Application.DataContracts.Dtos;
using Distributor.Application.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Distributor.Application.Services
{
    public class TemperatureService : ITemperatureService
    {
        private readonly IHubContext<TemperatureHub> hubContext;

        public TemperatureService(IHubContext<TemperatureHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public async Task NewTemperatureReceivedAsync(FlatTemperatureDto temperatureDto)
        {
            await hubContext.Clients.All.SendAsync("ReceiveTemperature", temperatureDto);
        }
    }
}