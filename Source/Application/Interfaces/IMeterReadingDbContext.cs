using Meter.Reading.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Meter.Reading.Application.Interfaces
{
    public interface IMeterReadingDbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public DbSet<MeterReading> Readings { get; set; }

        public DbSet<StagedMeterReading> StagedReadings { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        int SaveChanges();
    }
}
