﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApp.Data;

#nullable disable

namespace WebApp.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241119205538_AddVolume")]
    partial class AddVolume
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebApp.Models.BidAskTotal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AskVolume")
                        .HasColumnType("int");

                    b.Property<int>("BidVolume")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("TimeStamp")
                        .IsUnique();

                    b.ToTable("BidAskTotal");
                });

            modelBuilder.Entity("WebApp.Models.KlineData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("ClosePrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("HighPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Interval")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("LowPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("OpenPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("OpenTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Symbol")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("Volume")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("Symbol", "Interval", "OpenTime")
                        .IsUnique()
                        .HasFilter("[Symbol] IS NOT NULL AND [Interval] IS NOT NULL");

                    b.ToTable("KlineData");
                });
#pragma warning restore 612, 618
        }
    }
}
