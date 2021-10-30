using System;
using System.Collections.Generic;
using System.Text;

namespace Meter.Reading.Domain
{
    /// <summary>
    /// An entity representing the meter reading of a particular account
    /// </summary>
    public class MeterReading
    {
        /// <summary>
        /// Id of the account this reading belongs to.
        /// </summary>
        public long AccountId { get; set; }

        /// <summary>
        /// Date and time when the reading was taken
        /// </summary>
        public DateTimeOffset MeterReadingDateTimeUtc { get; set; }

        /// <summary>
        /// Reading value.
        /// </summary>
        public double MeterReadingValue { get; set; }

        /// <summary>
        /// Account this reading belogs to
        /// </summary>
        public Account Account { get; set; }
    }
}
