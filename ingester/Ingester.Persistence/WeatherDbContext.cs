using System;
using Ingester.Domain.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Ingester.Persistence
{
    public class WeatherDbContext : DbContext
    {
        public DbSet<DataPoint> DataPoints { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<Location> Locations { get; set; }

        public WeatherDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataPoint>()
                .HasOne(dataPoint => dataPoint.Sensor)
                .WithMany(sensor => sensor.DataPoints)
                .HasForeignKey(dataPoint => dataPoint.SensorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Sensor>()
                .HasOne(sensor => sensor.Location)
                .WithMany(location => location.Sensors)
                .HasForeignKey(sensor => sensor.LocationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
