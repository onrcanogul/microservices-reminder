﻿// <auto-generated />
using System;
using Microservices.CatalogInboxPublisherService.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Microservices.CatalogInboxPublisherService.Migrations
{
    [DbContext(typeof(CatalogInboxDbContext))]
    [Migration("20240807204038_mig_2")]
    partial class mig_2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microservices.CatalogInboxPublisherService.Models.CatalogInbox", b =>
                {
                    b.Property<Guid>("IdempotentToken")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Payload")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Processed")
                        .HasColumnType("bit");

                    b.HasKey("IdempotentToken");

                    b.ToTable("CatalogInboxes");
                });
#pragma warning restore 612, 618
        }
    }
}
