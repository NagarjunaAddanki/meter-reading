using Meter.Reading.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Meter.Reading.Application.Interfaces
{
    /// <summary>
    /// Interface to retrieve account data from Csv file
    /// </summary>
    public interface IAccountsCsvReader
    {
        /// <summary>
        /// Read and retrieve account data from csv file
        /// </summary>
        /// <param name="path">Path to csv file</param>
        /// <returns>List of account details</returns>
        List<Account> ReadAccountsFromCsv(string path);
    }
}
