namespace Ingester.Application.DataContracts.Dtos
{
    public class FlatTemperatureDto : TemperatureDto
    {
        public string SensorId { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
    }
}