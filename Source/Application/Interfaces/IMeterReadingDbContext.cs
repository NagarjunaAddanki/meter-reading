using Meter.Reading.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Meter.Reading.Application.Interfaces
{
    /// <summary>
    /// Interface representing the data store.
    /// </summary>
    public interface IMeterReadingDbContext
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

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        int SaveChanges();
    }
}
