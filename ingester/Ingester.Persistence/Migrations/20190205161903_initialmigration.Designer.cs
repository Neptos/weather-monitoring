﻿// <auto-generated />
using System;
using Ingester.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Ingester.Persistence.Migrations
{
    [DbContext(typeof(WeatherDbContext))]
    [Migration("20190205161903_initialmigration")]
    partial class initialmigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Ingester.Domain.Models.Location", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("Ingester.Domain.Models.Sensor", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("LocationId");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.ToTable("Sensors");
                });

            modelBuilder.Entity("Ingester.Domain.Models.Temperature", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("SensorId");

                    b.Property<DateTime>("Timestamp");

                    b.Property<float>("Value");

                    b.HasKey("Id");

                    b.HasIndex("SensorId");

                    b.ToTable("Temperatures");
                });

            modelBuilder.Entity("Ingester.Domain.Models.Sensor", b =>
                {
                    b.HasOne("Ingester.Domain.Models.Location", "Location")
                        .WithMany("Sensors")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Ingester.Domain.Models.Temperature", b =>
                {
                    b.HasOne("Ingester.Domain.Models.Sensor", "Sensor")
                        .WithMany("Temperatures")
                        .HasForeignKey("SensorId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
