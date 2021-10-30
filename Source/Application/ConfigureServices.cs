using Meter.Reading.Application.Business;
using Meter.Reading.Application.Filters;
using Meter.Reading.Application.Interfaces;
using Meter.Reading.Application.Validators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Meter.Reading.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplication(
           this IServiceCollection services,
           IConfiguration configuration)
        {
            services.AddScoped<IAccountsCsvReader, AccountsCsvReader>();
            services.AddScoped<IMeterReadingImporter, MeterReadingImporter>();
            services.AddScoped<IReadingFilter, MatchingAccountFilter>();

            return services;
        }
    }
}
