using Ingester.Application.Contracts;
using Ingester.Infrastructure.Consumers;
using Ingester.Infrastructure.Messages.Senders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ingester.IoC
{
    public static class ConfigureServices
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddSingleton<IHostedService, RabbitConsumer>();
            services.AddTransient<IDataPointPublisher, DataPointPublisher>();
        }
    }
}
