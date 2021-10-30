using Meter.Reading.Application.Interfaces;
using Meter.Reading.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;

namespace Meter.Reading.Infrastructure
{
    public class MeterReadingDbContext : DbContext , IMeterReadingDbContext
    {
        /// <summary>
        /// Energy accounts
        /// </summary>
        public DbSet<Account> Accounts { get; set; }

        /// <summary>
        /// Meter readings
        /// </summary>
        public DbSet<MeterReading> Readings { get; set; }

        /// <summary>
        /// Meter readings staged for population
        /// </summary>
        public DbSet<StagedMeterReading> StagedReadings { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options">Db creation options.</param>
        public MeterReadingDbContext(DbContextOptions<MeterReadingDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
