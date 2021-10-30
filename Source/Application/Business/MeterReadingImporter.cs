using CsvHelper;
using Meter.Reading.Application.Interfaces;
using Meter.Reading.Application.Validators;
using Meter.Reading.Application.ViewModels;
using Meter.Reading.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meter.Reading.Application.Business
{
    public class MeterReadingImporter : IMeterReadingImporter
    {
        private readonly IMeterReadingDbContext _meterReadingDbContext;
        private readonly List<IReadingFilter> _dataFilters;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="meterReadingDbContext"></param>
        public MeterReadingImporter(IMeterReadingDbContext meterReadingDbContext,
            IEnumerable<IReadingFilter> dataFilters)
        {
            _meterReadingDbContext = meterReadingDbContext;
            _dataFilters = dataFilters.ToList();
        }

        /// <summary>
        /// Import meter readings into data store.
        /// </summary>
        /// <param name="csvData">Meter reading in csv format</param>
        /// <returns>Import result</returns>
        public async Task<MeterReadingImportResult> ImportMeterData(string csvData)
        {
            MeterReadingImportResult result = null;

            //Read the CSV data and translate it to well formed
            //meter readings.
            var csvRecords = GetMeterReadingsFromCsvData(csvData);
            var meterReadings = TranslateMeterReadingFromCsv(csvRecords);

            //Filter out any meter readings which are not valid
            _dataFilters.ForEach(filter =>
            {
                meterReadings = filter.Filter(meterReadings);
            });

            //Add the valid meter readings to db.
            _meterReadingDbContext.Readings.AddRange(meterReadings);
            await _meterReadingDbContext.SaveChangesAsync();
            result = new MeterReadingImportResult
            {
                NumberOfReadingsImported = meterReadings.Count,
                NumberOfReadingsFailed = csvRecords.Count - meterReadings.Count,
            };

            return result;
        }

        private List<MeterReadingCsvViewModel> GetMeterReadingsFromCsvData(string csvData)
        {
            using (var reader = new StringReader(csvData))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<MeterReadingCsvViewModel>().ToList();
            }
        }

        /// <summary>
        /// Get Valid readings
        /// </summary>
        /// <param name="readings"></param>
        /// <returns></returns>
        private List<MeterReading> TranslateMeterReadingFromCsv(List<MeterReadingCsvViewModel> readings)
        {
            var result = new List<MeterReading>();

            readings.ForEach(reading =>
            {
                if (long.TryParse(reading.AccountId, out var accountId)

                //Date time must be in the given format.
                && DateTime.TryParseExact(reading.MeterReadingDateTime,
                        "d/MM/yyyy H:mm", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out var readTime)
                //Must have 5 digits
                && reading.MeterReadValue.Length == 5
                && double.TryParse(reading.MeterReadValue, out var value))
                {
                    result.Add(new MeterReading
                    {
                        AccountId = accountId,
                        ReadingDateTimeUtc = DateTime.SpecifyKind(readTime, DateTimeKind.Utc),
                        Value = value
                    });
                }
            });
            return result;
        }
    }
}
