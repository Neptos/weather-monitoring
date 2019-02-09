using System.Threading.Tasks;
using Ingester.Application.DataContracts.Dtos;

namespace Ingester.Application.Contracts
{
    public interface ITemperaturePublisher
    {
        Task Publish(FlatTemperatureDto flatTemperatureDto);
    }
}