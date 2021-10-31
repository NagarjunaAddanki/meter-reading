using Meter.Reading.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Meter.Reading.Tests
{
    internal class MockDbContext
    {
        public MeterReadingDbContext CreateMockLwpDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MeterReadingDbContext>();
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            var meterReadingContext = new MeterReadingDbContext(optionsBuilder.Options);
            SeedData(meterReadingContext);
            return meterReadingContext;
        }

        private void SeedData(MeterReadingDbContext dbContext)
        {
            dbContext.Accounts.Add(new Domain.Account
            {
                AccountId = 1,
                FirstName = "Harry",
                LastName = "Colas"
            });

            dbContext.Accounts.Add(new Domain.Account
            {
                AccountId = 2,
                FirstName = "Jeremy",
                LastName = "Sudo"
            });

            dbContext.Accounts.Add(new Domain.Account
            {
                AccountId = 3,
                FirstName = "Tom",
                LastName = "Neds"
            });

            dbContext.SaveChanges();
        }
    }
}
