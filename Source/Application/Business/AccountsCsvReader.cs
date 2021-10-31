using CsvHelper;
using Meter.Reading.Application.Interfaces;
using Meter.Reading.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Meter.Reading.Application.Business
{
    /// <inheritdoc />
    class AccountsCsvReader : IAccountsCsvReader
    {
        /// <inheritdoc />
        public List<Account> ReadAccountsFromCsv(string path)
        {
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<Account>().ToList();
            }
        }
    }
}
