using CsvHelper;
using Meter.Reading.Application.Interfaces;
using Meter.Reading.Application.Validators;
using Meter.Reading.Application.ViewModels;
using Meter.Reading.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meter.Reading.Application.Business
{
    /// <summary>
    /// Class to import meter reading data in csv format to data store.
    /// </summary>
    public class MeterReadingImporter : IMeterReadingImporter
    {
        /// <summary>
        /// Instance of database context.
        /// </summary>
        private readonly IMeterReadingDbContext _meterReadingDbContext;

        /// <summary>
        /// Logger instance
        /// </summary>
        private readonly ILogger<MeterReadingImporter> _logger;

        /// <summary>
        /// List of data filters to be applied on meter readings.
        /// </summary>
        private readonly List<IMeterReadingFilter> _dataFilters;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="meterReadingDbContext">Instance of db context</param>
        /// <param name="dataFilters">Data filters</param>
        public MeterReadingImporter(IMeterReadingDbContext meterReadingDbContext,
            IEnumerable<IMeterReadingFilter> dataFilters,
            ILogger<MeterReadingImporter> logger)
        {
            _meterReadingDbContext = meterReadingDbContext;
            this._logger = logger;
            _dataFilters = dataFilters.ToList();
        }

        /// <inheritdoc />
        public async Task<MeterReadingImportResult> ImportMeterData(string csvData)
        {
            var groupId = await StageMeterReadingsFromCsvData(csvData); //Stage
            await ApplyFilters(groupId); //Filter
            await DeployValidMeterReadings(groupId); //Deploy
            var result = await FlushStagedData(groupId); //Flush
            return result;
        }

        /// <summary>
        /// Stages the csv data for processing.
        /// </summary>
        /// <param name="csvData">Csv data to be imported</param>
        /// <returns></returns>
        private async Task<Guid> StageMeterReadingsFromCsvData(string csvData)
        {
            var groupId = Guid.NewGuid();
            using (var reader = new StringReader(csvData))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                //Though CSV helper supports directly deserializing from the file,
                //we have to do the manual reading as the date time structure is not
                //invariant in the provided csv file.
                var records = new List<StagedMeterReading>();
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    try
                    {
                        var record = new StagedMeterReading
                        {
                            GroupId = groupId,
                            AccountId = csv.GetField<long>(0),
                            MeterReadValue = csv.GetField(2).Trim()
                        };

                        //The data in csv is not invariant. Hence need to perform custom parsing.
                        if (DateTime.TryParseExact(csv.GetField(1).Trim(),
                            "d/MM/yyyy H:mm", CultureInfo.InvariantCulture,
                            DateTimeStyles.None, out var readTime))
                        {
                            record.MeterReadingDateTime = readTime;
                            records.Add(record);
                        }
                    }
                    catch(Exception e)
                    {
                        //If this CSV record could not be processed for some reason, log it and process the
                        //next row. The result will indicate that one or more rows have not made it to the data store.
                        _logger.LogError($"Invalid data encountered in the CSV file - {e.Message}");
                    }
                }

                await _meterReadingDbContext.StagedReadings.AddRangeAsync(records);
                await _meterReadingDbContext.SaveChangesAsync();
            }

            return groupId;
        }

        /// <summary>
        /// Apply data filters on the staged meter readings.
        /// </summary>
        /// <param name="groupId">Id of the group of readings which needs to be processed.</param>
        /// <returns>awaitable task</returns>
        private async Task ApplyFilters(Guid groupId)
        {
            foreach (var filter in _dataFilters)
            {
                await filter.FilterAsync(groupId);
            }
        }

        /// <summary>
        /// Moved valid meter readings from staging to final meter readings table.
        /// </summary>
        /// <param name="groupId">Id of the group of readings which needs to be processed.</param>
        /// <returns></returns>
        private async Task DeployValidMeterReadings(Guid groupId)
        {
            //Query all valid readings in this group.
            var validReadings = from stagedReading in _meterReadingDbContext.StagedReadings
                                where stagedReading.GroupId == groupId && stagedReading.IsValid
                                select stagedReading;

            var meterReadings = new List<MeterReading>();
            validReadings.ToList().ForEach(reading =>
            {
                meterReadings.Add(new MeterReading
                {
                    AccountId = reading.AccountId,
                    MeterReadingDateTimeUtc = reading.MeterReadingDateTime,
                    MeterReadingValue = Convert.ToDouble(reading.MeterReadValue)
                });
            });

            await _meterReadingDbContext.Readings.AddRangeAsync(meterReadings);
            await _meterReadingDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Remove the staged readings which have finished processing.
        /// </summary>
        /// <param name="groupId">Id of the group of readings which needs to be flushed out.</param>
        /// <returns></returns>
        private async Task<MeterReadingImportResult> FlushStagedData(Guid groupId)
        {
            var readingsInThisGroup = _meterReadingDbContext.StagedReadings.Where(r => r.GroupId == groupId);
            var result = new MeterReadingImportResult
            {
                NumberOfReadingsImported = readingsInThisGroup.Count(r => r.IsValid),
            };

            result.ValidationErrors.AddRange(readingsInThisGroup.Where(r => !r.IsValid).Select(r => r.Reason));

            _meterReadingDbContext.StagedReadings.RemoveRange(readingsInThisGroup);
            await _meterReadingDbContext.SaveChangesAsync();
            return result;
        }
    }
}
