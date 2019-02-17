using System;
using Distributor.Application.Contracts;
using Distributor.Application.Services;
using Distributor.Infrastructure.Consumers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Distributor.IoC
{
    public static class ConfigureServices
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddSingleton<IHostedService, RabbitConsumer>();
            services.AddTransient<ITemperatureService, TemperatureService>();
        }
    }
}
