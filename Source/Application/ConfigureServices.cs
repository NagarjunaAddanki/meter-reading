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

            //Filters expected on the meter readings.
            services.AddScoped<IMeterReadingFilter, DataFormatFilter>();
            services.AddScoped<IMeterReadingFilter, MatchingAccountFilter>();
            services.AddScoped<IMeterReadingFilter, DuplicateReadingFilter>();

            return services;
        }
    }
}
