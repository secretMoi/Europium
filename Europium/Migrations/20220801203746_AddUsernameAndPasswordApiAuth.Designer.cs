﻿// <auto-generated />
using Europium.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Europium.Migrations
{
    [DbContext(typeof(EuropiumContext))]
    [Migration("20220801203746_AddUsernameAndPasswordApiAuth")]
    partial class AddUsernameAndPasswordApiAuth
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Europium.Repositories.Models.ApiToMonitor", b =>
                {
                    b.Property<int>("ApiToMonitorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ApiToMonitorId"), 1L, 1);

                    b.Property<string>("ApiKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Logo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ApiToMonitorId");

                    b.ToTable("ApisToMonitor");
                });

            modelBuilder.Entity("Europium.Repositories.Models.ApiUrl", b =>
                {
                    b.Property<int>("ApiUrlId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ApiUrlId"), 1L, 1);

                    b.Property<int>("ApiToMonitorId")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ApiUrlId");

                    b.HasIndex("ApiToMonitorId");

                    b.ToTable("ApiUrls");
                });

            modelBuilder.Entity("Europium.Repositories.Models.ApiUrl", b =>
                {
                    b.HasOne("Europium.Repositories.Models.ApiToMonitor", "ApiToMonitor")
                        .WithMany("ApiUrls")
                        .HasForeignKey("ApiToMonitorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApiToMonitor");
                });

            modelBuilder.Entity("Europium.Repositories.Models.ApiToMonitor", b =>
                {
                    b.Navigation("ApiUrls");
                });
#pragma warning restore 612, 618
        }
    }
}
