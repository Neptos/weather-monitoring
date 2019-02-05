using System;
using Ingester.Domain.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Ingester.Persistence
{
    public class WeatherDbContext : DbContext
    {
        public DbSet<Temperature> Temperatures { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<Location> Locations { get; set; }

        public WeatherDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Temperature>()
                .HasOne(temperature => temperature.Sensor)
                .WithMany(sensor => sensor.Temperatures)
                .HasForeignKey(temperature => temperature.SensorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Sensor>()
                .HasOne(sensor => sensor.Location)
                .WithMany(location => location.Sensors)
                .HasForeignKey(sensor => sensor.LocationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
