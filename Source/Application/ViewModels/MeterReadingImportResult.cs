using System;
using System.Collections.Generic;
using System.Text;

namespace Meter.Reading.Application.ViewModels
{
    public class MeterReadingImportResult
    {
        public int NumberOfReadingsImported { get; set; }
        public List<string> ValidationErrors { get; set; } = new List<string>();
    }
}
