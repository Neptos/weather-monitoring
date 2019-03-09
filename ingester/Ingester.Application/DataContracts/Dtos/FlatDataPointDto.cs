namespace Ingester.Application.DataContracts.Dtos
{
    public class FlatDataPointDto : DataPointDto
    {
        public string SensorId { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
    }
}