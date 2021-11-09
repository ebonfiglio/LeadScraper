﻿// <auto-generated />
using LeadScraper.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LeadScraper.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20211109050722_SearchSettings")]
    partial class SearchSettings
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.0");

            modelBuilder.Entity("LeadScraper.Infrastructure.Entities.SearchSetting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CountryCode")
                        .HasColumnType("TEXT");

                    b.Property<int>("Pages")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ResultsPerPage")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SearchTerm")
                        .HasColumnType("TEXT");

                    b.Property<int>("StartingPage")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("SearchSetting");
                });

            modelBuilder.Entity("LeadScraper.Infrastructure.Entities.Setting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BingKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("BlackListTerms")
                        .HasColumnType("TEXT");

                    b.Property<string>("WhiteListTlds")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("LeadScraper.Infrastructure.Entities.WhoIsServer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Server")
                        .HasColumnType("TEXT");

                    b.Property<string>("Tld")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("WhoIsServers");
                });
#pragma warning restore 612, 618
        }
    }
}
