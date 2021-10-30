using Meter.Reading.Application.Interfaces;
using Meter.Reading.Application.Validators;
using Meter.Reading.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Meter.Reading.Application.Filters
{
    class DuplicateReadingFilter: BaseReadingFilter, IReadingFilter
    {
        public DuplicateReadingFilter(IMeterReadingDbContext meterReadingDbContext) : base(meterReadingDbContext)
        {

        }

        public List<MeterReading> Filter(List<MeterReading> meterReadings)
        {
            //Get the distinct account IDs
            var accountIds = meterReadings.Select(r => r.AccountId).Distinct();

            //Query for matching accounts from Db.
            var recognizedAccounts = MeterReadingDbContext.Readings.Where(a => accountIds.Contains(a.AccountId)).ToList();

            //Return only those meter readings for which account exists in db.
            return (from meterReading in meterReadings
                    join account in recognizedAccounts on meterReading.AccountId equals account.AccountId
                    select meterReading).ToList();
        }
    }
}
