using Meter.Reading.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Meter.Reading.Application.Interfaces
{
    public interface IMeterReadingImporter
    {
        Task<MeterReadingImportResult> ImportMeterData(string csvData);
    }
}
