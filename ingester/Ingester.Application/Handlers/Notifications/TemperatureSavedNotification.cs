using Ingester.Domain.Models;
using MediatR;

namespace Ingester.Application.Handlers.Notifications
{
    public class TemperatureSavedNotification : INotification
    {
        public Temperature Temperature { get; }

        public TemperatureSavedNotification(Temperature temperature)
        {
            Temperature = temperature;
        }
    }
}