using CsvHelper.Configuration;
using Meter.Reading.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Meter.Reading.Application.Business
{
    class MeterReadingCsvMap : ClassMap<StagedMeterReading>
    {
        public MeterReadingCsvMap()
        {
            Map(m => m.Id).Ignore();
            Map(m => m.IsValid).Ignore();
            Map(m => m.Reason).Ignore();
            Map(m => m.AccountId).Index(0);
            Map(m => m.MeterReadingDateTime).Index(1);
            Map(m => m.MeterReadValue).Index(2);
        }
    }
}