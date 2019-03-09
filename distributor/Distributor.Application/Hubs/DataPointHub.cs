using Distributor.Application.Contracts;
using Distributor.Application.DataContracts.Dtos;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Distributor.Application.Hubs
{
    public class DataPointHub : Hub
    {
        private readonly IServiceProvider serviceProvider;

        public DataPointHub(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task RetrieveDataPoints()
        {
            ICollection<FlatDataPointDto> dataPoints = new List<FlatDataPointDto>();
            using (var serviceScope = serviceProvider.CreateScope())
            {
                var dataPointService = serviceScope.ServiceProvider.GetRequiredService<IDataPointService>();
                dataPoints = await dataPointService.FetchCurrentDataPointsAsync();
            }
            foreach (var dataPointDto in dataPoints)
            {
                await Clients.Caller.SendAsync("ReceiveDataPoint", dataPointDto);
            }
        }
    }
}