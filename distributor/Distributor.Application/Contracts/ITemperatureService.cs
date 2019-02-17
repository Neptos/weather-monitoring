using System.Collections.Generic;
using System.Threading.Tasks;
using Distributor.Application.DataContracts.Dtos;

namespace Distributor.Application.Contracts
{
    public interface ITemperatureService
    {
        Task NewTemperatureReceivedAsync(FlatTemperatureDto temperatureDto);
        Task<ICollection<FlatTemperatureDto>> FetchCurrentTemperaturesAsync();
    }
}