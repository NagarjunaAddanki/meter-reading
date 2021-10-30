using System;
using System.Collections.Generic;
using System.Text;

namespace Meter.Reading.Domain
{
    public class StagedMeterReading
    {
        public long Id { get; set; }

        public Guid GroupId { get; set; }

        public long AccountId { get; set; }

        public DateTimeOffset MeterReadingDateTime { get; set; }

        public string MeterReadValue { get; set; }

        public bool IsValid { get; set; }

        public string Reason { get; set; }
    }
}
