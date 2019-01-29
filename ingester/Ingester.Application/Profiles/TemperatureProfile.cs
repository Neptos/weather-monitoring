using AutoMapper;
using Ingester.Application.DataContracts.Dtos;
using Ingester.Application.DataContracts.Requests;
using Ingester.Domain.Models;

namespace Ingester.Application.Profiles
{
    public class TemperatureProfile : Profile
    {
        public TemperatureProfile()
        {
            CreateMap<TemperatureRequest, Temperature>()
                .ForMember(temperature => temperature.Id, options => options.Ignore());
            CreateMap<Temperature, TemperatureDto>();
        }
    }
}