﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NeverovLab2backend.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NeverovLab2backend.Migrations
{
    [DbContext(typeof(pgDbContext))]
    [Migration("20230408071441_InitialDatabase")]
    partial class InitialDatabase
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0-preview.2.23128.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseSerialColumns(modelBuilder);

            modelBuilder.Entity("NeverovLab2backend.Models.Character", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Gender")
                        .HasColumnType("text");

                    b.Property<int?>("Id_Member")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Race")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Character");
                });

            modelBuilder.Entity("NeverovLab2backend.Models.Member", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("login")
                        .HasColumnType("text");

                    b.Property<string>("password")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Member");
                });

            modelBuilder.Entity("NeverovLab2backend.Models.Session", b =>
                {
                    b.Property<Guid?>("Id_Tale")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int?>("Id_Character")
                        .HasColumnType("integer");

                    b.HasKey("Id_Tale");

                    b.ToTable("Session");
                });

            modelBuilder.Entity("NeverovLab2backend.Models.Tale", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int?>("Id_Master")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<DateTime?>("Start_Tale")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("count_parties")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Tale");
                });
#pragma warning restore 612, 618
        }
    }
}
