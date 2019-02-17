using System.Collections.Generic;
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
        private readonly IRpcClient rpcClient;

        public TemperatureService(IHubContext<TemperatureHub> hubContext, IRpcClient rpcClient)
        {
            this.hubContext = hubContext;
            this.rpcClient = rpcClient;
        }

        public async Task<ICollection<FlatTemperatureDto>> FetchCurrentTemperaturesAsync()
        {
            return await Task.Run(() =>
             {
                 var dtos = rpcClient.FetchCurrentTemperatures();
                 return dtos;
             });
        }

        public async Task NewTemperatureReceivedAsync(FlatTemperatureDto temperatureDto)
        {
            await hubContext.Clients.All.SendAsync("ReceiveTemperature", temperatureDto);
        }
    }
}