using Meter.Reading.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Meter.Reading.Infrastructure.Configurations
{
    class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Accounts");

            //Primary key definition
            builder.Property(s => s.AccountId)
                //Important so that we can populate the account IDs instead of SQL generating automatically
                .ValueGeneratedNever();
        }
    }
}
