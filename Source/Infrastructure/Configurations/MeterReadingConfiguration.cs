using System;
using System.Collections.Generic;
using System.Text;
using Meter.Reading.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Meter.Reading.Infrastructure.Configurations
{
    public class MeterReadingConfiguration : IEntityTypeConfiguration<MeterReading>
    {
        public void Configure(EntityTypeBuilder<MeterReading> builder)
        {
            builder.ToTable("Readings");

            //Primary key definition
            builder.HasKey(s => new { s.AccountId, s.ReadingDateTimeUtc });

            //Define the relationship with account.
            builder.HasOne(lr => lr.Account).WithMany(l => l.Readings).HasForeignKey(lr => lr.AccountId);
        }
    }
}
