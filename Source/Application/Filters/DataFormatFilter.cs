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
                reading.Reason = !reading.IsValid ? $"Meter reading [{GetReadingIdentifier(reading)}] has invalid value [{reading.MeterReadValue}]. The meter readings must be 5 digit positive number." : string.Empty;
            });

            await MeterReadingDbContext.SaveChangesAsync();
        }

        private bool IsStagedReadingHasValidDataFormat(StagedMeterReading reading)
        {
            //Must be 5 digit positive numerical value.
            return reading.MeterReadValue.Length == 5
                && double.TryParse(reading.MeterReadValue, out var value)
                && value > 0;
        }
    }
}
