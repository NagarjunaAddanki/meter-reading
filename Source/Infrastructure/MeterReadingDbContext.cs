using Meter.Reading.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;

namespace Meter.Reading.Infrastructure
{
    public class MeterReadingDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public DbSet<MeterReading> Readings { get; set; }

        public MeterReadingDbContext(DbContextOptions<MeterReadingDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
