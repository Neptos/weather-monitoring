using System;
using Ingester.Domain.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Ingester.Persistence
{
    public class TemperatureDbContext : DbContext
    {
        public DbSet<Temperature> Temperatures { get; set; }
        
        public TemperatureDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
