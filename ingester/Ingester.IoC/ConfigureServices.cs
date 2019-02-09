using Ingester.Application.Contracts;
using Ingester.Infrastructure.Messages.Senders;
using Microsoft.Extensions.DependencyInjection;

namespace Ingester.IoC
{
    public static class ConfigureServices
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddTransient<ITemperaturePublisher, TemperaturePublisher>();
        }
    }
}
