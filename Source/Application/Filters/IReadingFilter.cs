using Meter.Reading.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Meter.Reading.Application.Validators
{
    public interface IReadingFilter
    {
        List<MeterReading> Filter(List<MeterReading> meterReadings);
    }
}
