using System;

namespace Ingester.Application.DataContracts.Requests
{
    public class RequestFilter
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Location { get; set; }
    }
}