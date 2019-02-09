using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Ingester.Infrastructure.Configurations;
using Ingester.IoC;
using Ingester.Persistence;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ingester.Presentation
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<RabbitConfiguration>(configuration.GetSection("Rabbit"));

            services.AddEntityFrameworkNpgsql()
                .AddDbContext<WeatherDbContext>(options =>
                {
                    options.UseNpgsql(configuration.GetConnectionString("PostgresDb"));
                })
                .BuildServiceProvider();

            services.AddAutoMapper(Assembly.GetEntryAssembly().GetReferencedAssemblies().Select(Assembly.Load));
            services.AddMediatR();
            services.AddOpenApiDocument();

            services.AddCustomServices();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IOptionsMonitor<RabbitConfiguration> rabbitConfigurationOptions)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUi3();

            app.UseMvc();
        }
    }
}
