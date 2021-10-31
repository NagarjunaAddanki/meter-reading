using Meter.Reading.Application.Interfaces;
using Meter.Reading.Application.Validators;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Meter.Reading.Application.Filters
{
    public class DuplicateReadingFilter : BaseReadingFilter, IMeterReadingFilter
    {
        public DuplicateReadingFilter(IMeterReadingDbContext meterReadingDbContext) : base(meterReadingDbContext)
        {

        }

        public async Task FilterAsync(Guid groupId)
        {
            //Query all valid readings in this group.
            var validReadings = from stagedReading in MeterReadingDbContext.StagedReadings
                                where stagedReading.GroupId == groupId && stagedReading.IsValid
                                select stagedReading;

            //Join with readings table to find duplicate readings.
            var duplicateReadings = from stagedReading in MeterReadingDbContext.StagedReadings
                                    join meterReading in MeterReadingDbContext.Readings
                                    on new { stagedReading.AccountId, stagedReading.MeterReadingDateTime } equals
                                    new { meterReading.AccountId, MeterReadingDateTime = meterReading.MeterReadingDateTimeUtc }
                                    where stagedReading.GroupId == groupId && stagedReading.IsValid
                                    select stagedReading.Id;

            //Filter out anything which do not matches.
            validReadings.ToList().ForEach(reading =>
            {
                reading.IsValid = !duplicateReadings.Contains(reading.Id);
            });

            await MeterReadingDbContext.SaveChangesAsync();
        }
    }
}
