using Meter.Reading.Application.Interfaces;
using Meter.Reading.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;

namespace Meter.Reading.Infrastructure
{
    /// <summary>
    /// Data store for meter readings.
    /// </summary>
    public class MeterReadingDbContext : DbContext , IMeterReadingDbContext
    {
        /// <inheritdoc />
        public DbSet<Account> Accounts { get; set; }

        /// <inheritdoc />
        public DbSet<MeterReading> Readings { get; set; }

        /// <inheritdoc />
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
