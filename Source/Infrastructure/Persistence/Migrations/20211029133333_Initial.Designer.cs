﻿// <auto-generated />
using System;
using Meter.Reading.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Meter.Reading.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(MeterReadingDbContext))]
    [Migration("20211029133333_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Meter.Reading.Domain.Account", b =>
                {
                    b.Property<long>("AccountId")
                        .HasColumnType("bigint");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AccountId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Meter.Reading.Domain.MeterReading", b =>
                {
                    b.Property<long>("AccountId")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset>("ReadingDateTimeUtc")
                        .HasColumnType("datetimeoffset");

                    b.Property<double>("Value")
                        .HasColumnType("float");

                    b.HasKey("AccountId", "ReadingDateTimeUtc");

                    b.ToTable("Readings");
                });

            modelBuilder.Entity("Meter.Reading.Domain.MeterReading", b =>
                {
                    b.HasOne("Meter.Reading.Domain.Account", "Account")
                        .WithMany("Readings")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Meter.Reading.Domain.Account", b =>
                {
                    b.Navigation("Readings");
                });
#pragma warning restore 612, 618
        }
    }
}
