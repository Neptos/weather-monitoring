using System.Collections.Generic;
using System.Threading.Tasks;
using Distributor.Application.Contracts;
using Distributor.Application.DataContracts.Dtos;
using Distributor.Application.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Distributor.Application.Services
{
    public class DataPointService : IDataPointService
    {
        private readonly IHubContext<DataPointHub> hubContext;
        private readonly IRpcClient rpcClient;

        public DataPointService(IHubContext<DataPointHub> hubContext, IRpcClient rpcClient)
        {
            this.hubContext = hubContext;
            this.rpcClient = rpcClient;
        }

        public async Task<ICollection<FlatDataPointDto>> FetchCurrentDataPointsAsync()
        {
            return await Task.Run(() =>
             {
                 var dtos = rpcClient.FetchCurrentDataPoints();
                 return dtos;
             });
        }

        public async Task NewDataPointReceivedAsync(FlatDataPointDto dataPointDto)
        {
            await hubContext.Clients.All.SendAsync("ReceiveDataPoint", dataPointDto);
        }
    }
}