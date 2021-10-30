using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Meter.Reading.Application.ViewModels
{
    class MeterReadingCsvViewModel
    {
        [Index(0)]
        public string AccountId { get; set; }

        [Index(1)]
        public string MeterReadingDateTime { get; set; }

        [Index(2)]
        public string MeterReadValue { get; set; }

        [Ignore]
        public bool IsValid { get; set; }
    }
}
