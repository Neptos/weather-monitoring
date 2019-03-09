using System.Collections.Generic;
using System.Threading.Tasks;
using Distributor.Application.DataContracts.Dtos;

namespace Distributor.Application.Contracts
{
    public interface IDataPointService
    {
        Task NewDataPointReceivedAsync(FlatDataPointDto dataPointDto);
        Task<ICollection<FlatDataPointDto>> FetchCurrentDataPointsAsync();
    }
}