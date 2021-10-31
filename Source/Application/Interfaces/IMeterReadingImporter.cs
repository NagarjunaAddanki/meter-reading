using Meter.Reading.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Meter.Reading.Application.Interfaces
{
    public interface IMeterReadingImporter
    {
        /// <summary>
        /// Import meter readings into data store.
        /// </summary>
        /// <param name="csvData">Meter reading in csv format</param>
        /// <returns>Import result</returns>
        Task<MeterReadingImportResult> ImportMeterData(string csvData);
    }
}
