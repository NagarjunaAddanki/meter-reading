using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Meter.Reading.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructure(
           this IServiceCollection services,
           IConfiguration configuration)
        {
            services.AddDbContext<MeterReadingDbContext>(options => {
                options.UseSqlServer(configuration.GetConnectionString("MeterReadingDbConnection"));
            });

            return services;
        }
    }
}
