using Meter.Reading.Application.Interfaces;
using Meter.Reading.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Meter.Reading.Application.Validators
{
    public interface IMeterReadingFilter
    {
        Task FilterAsync(Guid groupId);
    }
}
