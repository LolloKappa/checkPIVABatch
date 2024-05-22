﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using checkPIVABatch.Models;

#nullable disable

namespace checkPIVABatch.Migrations
{
    [DbContext(typeof(CheckIVABatchDBContext))]
    [Migration("20240522192959_AddCountryCodeAndVatNumberUniqueIndex")]
    partial class AddCountryCodeAndVatNumberUniqueIndex
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("checkPIVABatch.Models.TaxInterrogationHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("address");

                    b.Property<string>("CountryCode")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("countryCode");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("name");

                    b.Property<DateTime>("RequestDate")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("requestDate");

                    b.Property<string>("RequestIdentifier")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("requestIdentifier");

                    b.Property<bool>("Valid")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("valid");

                    b.Property<string>("VatNumber")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("vatNumber");

                    b.HasKey("Id");

                    b.HasIndex("CountryCode", "VatNumber")
                        .IsUnique();

                    b.ToTable("taxInterrogationHistory", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
