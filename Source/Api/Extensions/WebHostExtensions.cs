using Meter.Reading.Application.Interfaces;
using Meter.Reading.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meter.Reading.Api.Extensions
{
    public static class WebHostExtensions
    {
        public static IHost RunMigrations(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var logger = scope.ServiceProvider.GetService<ILogger<Program>>();

            try
            {
                // create database context using migration db credentials
                var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var migrationConnectionString = config.GetConnectionString("MeterReadingDbConnection");

                var optionsBuilder = new DbContextOptionsBuilder<MeterReadingDbContext>();
                optionsBuilder.UseSqlServer(migrationConnectionString);
                using var databaseContext = new MeterReadingDbContext(optionsBuilder.Options);
                var database = databaseContext.Database;

                logger.LogInformation("Migrating the database...");
                database.Migrate();
                logger.LogInformation("Migrating the database [complete]");
            }
            catch (Exception e)
            {
                logger.LogError($"Failed to run migrations - {e.Message}");
                throw;
            }

            return host;
        }

        public static IHost SeedData(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var logger = scope.ServiceProvider.GetService<ILogger<Program>>();
                try
                {
                    // Now that the database is up to date. Let's seed
                    var context = scope.ServiceProvider.GetRequiredService<MeterReadingDbContext>();
                    var accountsCsvReader = scope.ServiceProvider.GetRequiredService<IAccountsCsvReader>();

                    if (!context.Accounts.Any())
                    {
                        foreach (var account in accountsCsvReader.ReadAccountsFromCsv(@"Data/Accounts.csv"))
                        {
                            context.Accounts.Add(account);
                        }
                        context.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    logger.LogError($"Failed to seed database with accounts data - {e.Message}");
                    throw;
                }
            }

            return host;
        }
    }
}
