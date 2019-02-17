using Distributor.Application.Contracts;
using Distributor.Application.DataContracts.Dtos;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Distributor.Application.Hubs
{
    public class TemperatureHub : Hub
    {
        private readonly IServiceProvider serviceProvider;

        public TemperatureHub(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task RetrieveTemperatures()
        {
            ICollection<FlatTemperatureDto> temperatures = new List<FlatTemperatureDto>();
            using (var serviceScope = serviceProvider.CreateScope())
            {
                var temperatureService = serviceScope.ServiceProvider.GetRequiredService<ITemperatureService>();
                temperatures = await temperatureService.FetchCurrentTemperaturesAsync();
            }
            foreach (var temperatureDto in temperatures)
            {
                await Clients.Caller.SendAsync("ReceiveTemperature", temperatureDto);
            }
        }
    }
}