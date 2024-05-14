﻿// <auto-generated />
using System;
using FafCarsApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FafCarsApi.Migrations
{
    [DbContext(typeof(FafCarsDbContext))]
    partial class FafCarsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FafCarsApi.Models.Entities.Listing", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("BrandName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("brand");

                    b.Property<int?>("Clearance")
                        .HasColumnType("integer")
                        .HasColumnName("clearance");

                    b.Property<string>("Color")
                        .HasMaxLength(7)
                        .HasColumnType("character varying(7)")
                        .HasColumnName("color");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TIMESTAMP(0) WITHOUT TIME ZONE")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("TIMESTAMP(0) WITHOUT TIME ZONE")
                        .HasColumnName("deleted_at");

                    b.Property<string>("EngineType")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("engine_type");

                    b.Property<double?>("EngineVolume")
                        .HasColumnType("double precision")
                        .HasColumnName("engine_volume");

                    b.Property<int?>("Horsepower")
                        .HasColumnType("integer")
                        .HasColumnName("horsepower");

                    b.Property<int?>("Mileage")
                        .HasColumnType("integer")
                        .HasColumnName("mileage");

                    b.Property<string>("ModelName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("model");

                    b.Property<string>("PreviewFileName")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("preview_file_name");

                    b.Property<double>("Price")
                        .HasColumnType("double precision")
                        .HasColumnName("price");

                    b.Property<Guid>("PublisherId")
                        .HasColumnType("uuid")
                        .HasColumnName("publisher_id");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("type");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TIMESTAMP(0) WITHOUT TIME ZONE")
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<int?>("WheelSize")
                        .HasColumnType("integer")
                        .HasColumnName("wheel_size");

                    b.Property<int?>("Year")
                        .HasColumnType("integer")
                        .HasColumnName("year");

                    b.HasKey("Id");

                    b.HasIndex("PublisherId");

                    b.ToTable("listings");

                    b.HasData(
                        new
                        {
                            Id = new Guid("90bf64a9-fffc-4e14-ba3d-85f1bf93aa63"),
                            BrandName = "BMW",
                            Clearance = 210,
                            Color = "#FFFFFF",
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DeletedAt = new DateTime(2024, 5, 14, 12, 16, 35, 115, DateTimeKind.Local).AddTicks(8278),
                            EngineType = "Petrol",
                            EngineVolume = 3.0,
                            Horsepower = 300,
                            Mileage = 15000,
                            ModelName = "X5",
                            Price = 35000.0,
                            PublisherId = new Guid("10ca3d19-7151-405c-bd76-20957bb92a05"),
                            Type = "SUV",
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            WheelSize = 20,
                            Year = 2018
                        },
                        new
                        {
                            Id = new Guid("b88c3f76-d8e8-49b9-8878-06eb64dbb3f2"),
                            BrandName = "Toyota",
                            Clearance = 170,
                            Color = "#007A5E",
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DeletedAt = new DateTime(2024, 5, 14, 12, 16, 35, 118, DateTimeKind.Local).AddTicks(3264),
                            EngineType = "Hybrid",
                            EngineVolume = 2.5,
                            Horsepower = 208,
                            Mileage = 10000,
                            ModelName = "Camry",
                            Price = 25000.0,
                            PublisherId = new Guid("10ca3d19-7151-405c-bd76-20957bb92a05"),
                            Type = "Sedan",
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            WheelSize = 18,
                            Year = 2020
                        },
                        new
                        {
                            Id = new Guid("299cc158-a59d-44bd-9353-d0137acc56fe"),
                            BrandName = "Ford",
                            Clearance = 230,
                            Color = "#FF0000",
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EngineType = "Gasoline",
                            EngineVolume = 3.5,
                            Horsepower = 375,
                            Mileage = 25000,
                            ModelName = "F-150",
                            Price = 30000.0,
                            PublisherId = new Guid("10ca3d19-7151-405c-bd76-20957bb92a05"),
                            Type = "Truck",
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            WheelSize = 17,
                            Year = 2019
                        },
                        new
                        {
                            Id = new Guid("6215f7d1-c55d-42e0-8f1b-f2b7e6b3248a"),
                            BrandName = "Honda",
                            Clearance = 160,
                            Color = "#002366",
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EngineType = "Gasoline",
                            EngineVolume = 1.8,
                            Horsepower = 174,
                            Mileage = 20000,
                            ModelName = "Civic",
                            Price = 18000.0,
                            PublisherId = new Guid("d06cbfa0-6f02-455c-a3cd-a32f1f22b64b"),
                            Type = "Sedan",
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            WheelSize = 16,
                            Year = 2017
                        },
                        new
                        {
                            Id = new Guid("fc0d2e50-213e-45c0-bf2f-cfe7a1259dba"),
                            BrandName = "Mercedes-Benz",
                            Clearance = 180,
                            Color = "#1C1C1C",
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EngineType = "Diesel",
                            EngineVolume = 2.0,
                            Horsepower = 240,
                            Mileage = 18000,
                            ModelName = "E-Class",
                            Price = 40000.0,
                            PublisherId = new Guid("d06cbfa0-6f02-455c-a3cd-a32f1f22b64b"),
                            Type = "Sedan",
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            WheelSize = 18,
                            Year = 2019
                        },
                        new
                        {
                            Id = new Guid("5bb7b41f-33bc-4fed-9b1b-9c81596b30b8"),
                            BrandName = "Chevrolet",
                            Clearance = 250,
                            Color = "#800000",
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EngineType = "Gasoline",
                            EngineVolume = 5.2999999999999998,
                            Horsepower = 355,
                            Mileage = 12000,
                            ModelName = "Silverado",
                            Price = 38000.0,
                            PublisherId = new Guid("d06cbfa0-6f02-455c-a3cd-a32f1f22b64b"),
                            Type = "Truck",
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            WheelSize = 20,
                            Year = 2021
                        },
                        new
                        {
                            Id = new Guid("68a68b4c-c4e0-4d15-bf37-3559abf10deb"),
                            BrandName = "Chevrolet",
                            Clearance = 250,
                            Color = "#800000",
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EngineType = "Gasoline",
                            EngineVolume = 5.2999999999999998,
                            Horsepower = 355,
                            Mileage = 12000,
                            ModelName = "Silverado",
                            Price = 38000.0,
                            PublisherId = new Guid("d06cbfa0-6f02-455c-a3cd-a32f1f22b64b"),
                            Type = "Truck",
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            WheelSize = 20,
                            Year = 2021
                        },
                        new
                        {
                            Id = new Guid("94ed8176-3f94-4a79-bce5-e638fc857bf5"),
                            BrandName = "Ford",
                            Clearance = 230,
                            Color = "#0000FF",
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EngineType = "Gasoline",
                            EngineVolume = 5.0,
                            Horsepower = 375,
                            Mileage = 10000,
                            ModelName = "F-150",
                            Price = 42000.0,
                            PublisherId = new Guid("d06cbfa0-6f02-455c-a3cd-a32f1f22b64b"),
                            Type = "Truck",
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            WheelSize = 18,
                            Year = 2022
                        },
                        new
                        {
                            Id = new Guid("0fbb77d0-dd87-472a-9124-978539b5ef30"),
                            BrandName = "Toyota",
                            Clearance = 240,
                            Color = "#006400",
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EngineType = "Gasoline",
                            EngineVolume = 3.5,
                            Horsepower = 278,
                            Mileage = 15000,
                            ModelName = "Tacoma",
                            Price = 34000.0,
                            PublisherId = new Guid("d06cbfa0-6f02-455c-a3cd-a32f1f22b64b"),
                            Type = "Truck",
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            WheelSize = 17,
                            Year = 2020
                        },
                        new
                        {
                            Id = new Guid("710c75ad-32c7-40b5-905b-8646d186c328"),
                            BrandName = "Honda",
                            Clearance = 150,
                            Color = "#FFA500",
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EngineType = "Gasoline",
                            EngineVolume = 2.0,
                            Horsepower = 174,
                            Mileage = 20000,
                            ModelName = "Civic",
                            Price = 22000.0,
                            PublisherId = new Guid("d06cbfa0-6f02-455c-a3cd-a32f1f22b64b"),
                            Type = "Sedan",
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            WheelSize = 16,
                            Year = 2019
                        },
                        new
                        {
                            Id = new Guid("c81d7dff-25e3-43a3-a327-459ad75d1a1e"),
                            BrandName = "BMW",
                            Clearance = 140,
                            Color = "#000000",
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EngineType = "Gasoline",
                            EngineVolume = 2.0,
                            Horsepower = 255,
                            Mileage = 18000,
                            ModelName = "3 Series",
                            Price = 35000.0,
                            PublisherId = new Guid("d06cbfa0-6f02-455c-a3cd-a32f1f22b64b"),
                            Type = "Sedan",
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            WheelSize = 18,
                            Year = 2020
                        },
                        new
                        {
                            Id = new Guid("0677eb44-0d0e-4f43-8e01-ecaec5e91fde"),
                            BrandName = "Tesla",
                            Clearance = 160,
                            Color = "#FFFFFF",
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EngineType = "Electric",
                            EngineVolume = 0.0,
                            Horsepower = 450,
                            Mileage = 5000,
                            ModelName = "Model 3",
                            Price = 50000.0,
                            PublisherId = new Guid("d06cbfa0-6f02-455c-a3cd-a32f1f22b64b"),
                            Type = "Sedan",
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            WheelSize = 19,
                            Year = 2021
                        });
                });

            modelBuilder.Entity("FafCarsApi.Models.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("email");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("character varying(60)")
                        .HasColumnName("password");

                    b.Property<int[]>("Roles")
                        .IsRequired()
                        .HasColumnType("integer[]")
                        .HasColumnName("roles");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.ToTable("users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("10ca3d19-7151-405c-bd76-20957bb92a05"),
                            Email = "warek@gmail.com",
                            Password = "$2a$13$Ds7Nitf/NsuilEtVtM9svePvptmYKeuY821oIo2gv.5yG9ggPxkiq",
                            Roles = new[] { 1 },
                            Username = "warek"
                        },
                        new
                        {
                            Id = new Guid("d06cbfa0-6f02-455c-a3cd-a32f1f22b64b"),
                            Email = "denis@gmail.com",
                            Password = "$2a$13$qB8B9Eg30n/SlnImSHL9k.bMecO5ibCaJhqflB4pok5XWJQEuVMAG",
                            Roles = new[] { 0, 3 },
                            Username = "denis"
                        });
                });

            modelBuilder.Entity("FafCarsApi.Models.Entities.Listing", b =>
                {
                    b.HasOne("FafCarsApi.Models.Entities.User", "Publisher")
                        .WithMany("Listings")
                        .HasForeignKey("PublisherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Publisher");
                });

            modelBuilder.Entity("FafCarsApi.Models.Entities.User", b =>
                {
                    b.Navigation("Listings");
                });
#pragma warning restore 612, 618
        }
    }
}
