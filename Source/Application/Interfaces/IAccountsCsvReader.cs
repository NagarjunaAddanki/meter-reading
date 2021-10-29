using Meter.Reading.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Meter.Reading.Application.Interfaces
{
    public interface IAccountsCsvReader
    {
        List<Account> ReadAccountsFromCsv(string path);
    }
}
