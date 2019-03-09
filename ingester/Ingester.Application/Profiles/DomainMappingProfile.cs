using AutoMapper;
using Ingester.Application.DataContracts.Dtos;
using Ingester.Application.DataContracts.Requests;
using Ingester.Domain.Models;

namespace Ingester.Application.Profiles
{
    public class DomainMappingProfile : Profile
    {
        public DomainMappingProfile()
        {
            CreateMap<DataPointRequest, DataPoint>()
                .ForMember(dataPoint => dataPoint.Id, options => options.Ignore());
            
            CreateMap<DataPoint, DataPointDto>();

            CreateMap<DataPoint, FlatDataPointDto>()
                .ForMember(dto => dto.LocationId, options => options.MapFrom(dp => dp.Sensor.LocationId))
                .ForMember(dto => dto.LocationName, options => options.MapFrom(dp => dp.Sensor.Location.Name));
        }
    }
}
