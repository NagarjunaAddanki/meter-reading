using Meter.Reading.Application.Interfaces;
using Meter.Reading.Application.Validators;
using Meter.Reading.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meter.Reading.Application.Filters
{
    public class DataFormatFilter : BaseReadingFilter, IMeterReadingFilter
    {
        public DataFormatFilter(IMeterReadingDbContext meterReadingDbContext) : base(meterReadingDbContext)
        {

        }

        public async Task FilterAsync(Guid groupId)
        {
            var stagedReadings = MeterReadingDbContext.StagedReadings.Where(r => r.GroupId == groupId);
            stagedReadings.ToList().ForEach(reading =>
            {
                reading.IsValid = IsStagedReadingHasValidDataFormat(reading);
            });

            await MeterReadingDbContext.SaveChangesAsync();
        }

        private bool IsStagedReadingHasValidDataFormat(StagedMeterReading reading)
        {
            //Must have 5 digits
            return reading.MeterReadValue.Length == 5
                && double.TryParse(reading.MeterReadValue, out var value);
        }
    }
}
