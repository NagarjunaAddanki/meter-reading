using System;
using System.Collections.Generic;
using System.Text;
using Meter.Reading.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Meter.Reading.Infrastructure.Configurations
{
    public class StagedMeterReadingConfiguration : IEntityTypeConfiguration<StagedMeterReading>
    {
        public void Configure(EntityTypeBuilder<StagedMeterReading> builder)
        {
            builder.ToTable("StagedMeterReadings");

            //Primary key definition
            builder.HasKey(s => s.Id);
        }
    }
}
