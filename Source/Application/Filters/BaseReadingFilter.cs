using Meter.Reading.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Meter.Reading.Application.Filters
{
    abstract class BaseReadingFilter
    {
        protected readonly IMeterReadingDbContext MeterReadingDbContext;

        protected BaseReadingFilter(IMeterReadingDbContext meterReadingDbContext)
        {
            MeterReadingDbContext = meterReadingDbContext;
        }
    }
}
