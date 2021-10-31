using Meter.Reading.Application.Interfaces;
using Meter.Reading.Application.Validators;
using Meter.Reading.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meter.Reading.Application.Filters
{
    public class MatchingAccountFilter : BaseReadingFilter, IMeterReadingFilter
    {
        public MatchingAccountFilter(IMeterReadingDbContext meterReadingDbContext) : base(meterReadingDbContext)
        {

        }

        public async Task FilterAsync(Guid groupId)
        {
            //Query all valid readings in this group.
            var validReadings = from stagedReading in MeterReadingDbContext.StagedReadings
                                where stagedReading.GroupId == groupId && stagedReading.IsValid
                                select stagedReading;

            //Join with accounts table
            var matchedReadings = from stagedReading in MeterReadingDbContext.StagedReadings
                                  join account in MeterReadingDbContext.Accounts
                                 on stagedReading.AccountId equals account.AccountId
                                 where stagedReading.GroupId == groupId && stagedReading.IsValid
                                 select stagedReading.Id;

            //Filter out anything which do not matches.
            validReadings.ToList().ForEach(reading =>
            {
                reading.IsValid = matchedReadings.Contains(reading.Id);
            });

            await MeterReadingDbContext.SaveChangesAsync();
        }
    }
}
