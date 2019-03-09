using System.Collections.Generic;
using System.Threading.Tasks;
using Distributor.Application.DataContracts.Dtos;

namespace Distributor.Application.Contracts
{
    public interface IRpcClient
    {
        ICollection<FlatDataPointDto> FetchCurrentDataPoints();
    }
}