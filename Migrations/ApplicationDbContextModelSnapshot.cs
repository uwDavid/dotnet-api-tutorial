﻿// <auto-generated />
using System;
using ApiDemo.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ApiDemo.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ApiDemo.Models.Shirt", b =>
                {
                    b.Property<int>("ShirtId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ShirtId"));

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Color")
                        .HasColumnType("text");

                    b.Property<string>("Gender")
                        .HasColumnType("text");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<int?>("Size")
                        .HasColumnType("integer");

                    b.HasKey("ShirtId");

                    b.ToTable("Shirts");

                    b.HasData(
                        new
                        {
                            ShirtId = 1,
                            Brand = "Levi's",
                            Color = "Blue",
                            Gender = "Men",
                            Price = 30.0,
                            Size = 10
                        },
                        new
                        {
                            ShirtId = 2,
                            Brand = "MLH",
                            Color = "Red",
                            Gender = "Men",
                            Price = 35.0,
                            Size = 12
                        },
                        new
                        {
                            ShirtId = 3,
                            Brand = "Apple",
                            Color = "White",
                            Gender = "Women",
                            Price = 22.0,
                            Size = 8
                        },
                        new
                        {
                            ShirtId = 4,
                            Brand = "Nike",
                            Color = "Black",
                            Gender = "Women",
                            Price = 33.0,
                            Size = 9
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
