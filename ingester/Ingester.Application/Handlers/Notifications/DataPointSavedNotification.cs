using Ingester.Domain.Models;
using MediatR;

namespace Ingester.Application.Handlers.Notifications
{
    public class DataPointSavedNotification : INotification
    {
        public DataPoint DataPoint { get; }

        public DataPointSavedNotification(DataPoint dataPoint)
        {
            DataPoint = dataPoint;
        }
    }
}