using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Meter.Reading.Domain
{
    public class Account
    {
        /// <summary>
        /// Account Id of the user
        /// </summary>
        public long AccountId { get; set; }

        /// <summary>
        /// First name of the user.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of the user
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// List of readings for this account
        /// </summary>
        public List<MeterReading> Readings { get; set; }
    }
}
