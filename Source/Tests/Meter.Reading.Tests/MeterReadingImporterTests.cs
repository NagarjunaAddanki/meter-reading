using FluentAssertions;
using Meter.Reading.Application.Business;
using Meter.Reading.Application.Filters;
using Meter.Reading.Application.Validators;
using Meter.Reading.Domain;
using Meter.Reading.Infrastructure;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meter.Reading.Tests
{
    [TestFixture]
    [Description("Test class to validate the HL7 message parsing assumptions")]
    public class Tests
    {
        private MeterReadingDbContext _dbContext;

        private List<IMeterReadingFilter> _filters;
        private MeterReadingImporter _importer;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            //Mock db context
            _dbContext = new MockDbContext().CreateMockLwpDbContext();
            _filters = new List<IMeterReadingFilter>
            {
                new DataFormatFilter(_dbContext),
                new MatchingAccountFilter(_dbContext),
                new DuplicateReadingFilter(_dbContext)
            };

            _importer = new MeterReadingImporter(_dbContext, _filters);
        }

        [SetUp]
        public void Init()
        {
            _dbContext.Readings.RemoveRange(_dbContext.Readings);
            _dbContext.StagedReadings.RemoveRange(_dbContext.StagedReadings);
        }

        [Test]
        [Description("Test the all valid values are inserted in to data store")]
        public async Task MustInsertAllValidMeterReadings()
        {
            //arrange
            var csvData = GetCsvString(ValidReadings);

            //Act
            await _importer.ImportMeterData(csvData);

            //Assert
            _dbContext.Readings.Count().Should().Be(3);
            _dbContext.StagedReadings.Count().Should().Be(0);
        }

        [Test]
        [TestCase("ABC")] //Not a number
        [TestCase("12")] //Not a NNNNN format
        [Description("Test the no readings with invalid reading values inserted in to data store")]
        public async Task MustFailWhenReadingValueIsInvalid(string readingValue)
        {
            //arrange
            var readings = ValidReadings;
            readings.Add(new StagedMeterReading
            {
                AccountId = 1,
                MeterReadingDateTime = DateTimeOffset.UtcNow,
                MeterReadValue = readingValue
            });

            var csvData = GetCsvString(ValidReadings);

            //Act
            await _importer.ImportMeterData(csvData);

            //Assert
            readings.Count.Should().Be(4); //Total data sent to the importer
            _dbContext.Readings.Count().Should().Be(3); //The invalid data is ignored.
            _dbContext.StagedReadings.Count().Should().Be(0);
        }

        [Test]
        [TestCase(99999)]
        [Description("Test the no readings without matching account id is inserted in to data store")]
        public async Task MustFailWhenAccountIdDoNotExist(long accountId)
        {
            //arrange
            var readings = ValidReadings;
            readings.Add(new StagedMeterReading
            {
                AccountId = accountId,
                MeterReadingDateTime = DateTimeOffset.UtcNow,
                MeterReadValue = "12345"
            });

            var csvData = GetCsvString(ValidReadings);

            //Act
            await _importer.ImportMeterData(csvData);

            //Assert
            readings.Count.Should().Be(4); //Total data sent to the importer
            _dbContext.Readings.Count().Should().Be(3); //The invalid data is ignored.
            _dbContext.StagedReadings.Count().Should().Be(0);
        }

        [Test]
        [Description("Test that duplicate readings are not inserted in to data store")]
        public async Task MustFailWhenDuplicateRecordsAreImported()
        {
            //arrange
            var csvData = GetCsvString(ValidReadings);

            //Act
            await _importer.ImportMeterData(csvData);

            //Assert
            _dbContext.Readings.Count().Should().Be(3);
            _dbContext.StagedReadings.Count().Should().Be(0);

            //Act - Import the same data once again
            await _importer.ImportMeterData(csvData);

            //Assert - That when same data is imported twice, duplicate records
            //are not created
            _dbContext.Readings.Count().Should().Be(3);
            _dbContext.StagedReadings.Count().Should().Be(0);

        }

        /// <summary>
        /// Get the CSV string from the test readings.
        /// </summary>
        /// <param name="meterReadings">Collection of test readings data</param>
        /// <returns></returns>
        private string GetCsvString(List<StagedMeterReading> meterReadings)
        {
            var sb = new StringBuilder();
            sb.AppendLine("AccountId, MeterReadingDateTime, MeterReadValue,");
            meterReadings.ForEach(reading =>
                sb.AppendLine($"{reading.AccountId}, {reading.MeterReadingDateTime.ToString("d/MM/yyyy H:mm")}, {reading.MeterReadValue}"));
            return sb.ToString();
        }

        /// <summary>
        /// Valid test readings.
        /// </summary>
        private List<StagedMeterReading> ValidReadings => new List<StagedMeterReading>
            {
                new StagedMeterReading
                {
                    AccountId = 1,
                    MeterReadingDateTime = DateTimeOffset.UtcNow,
                    MeterReadValue = "12345"
                },
                new StagedMeterReading
                {
                    AccountId = 2,
                    MeterReadingDateTime = DateTimeOffset.UtcNow,
                    MeterReadValue = "12345"
                },new StagedMeterReading
                {
                    AccountId = 3,
                    MeterReadingDateTime = DateTimeOffset.UtcNow,
                    MeterReadValue = "12345"
                }
            };
    }
}