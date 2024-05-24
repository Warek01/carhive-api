﻿// <auto-generated />
using System;
using System.Collections.Generic;
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

                    b.Property<List<string>>("Images")
                        .IsRequired()
                        .HasColumnType("text[]")
                        .HasColumnName("images");

                    b.Property<int?>("Mileage")
                        .HasColumnType("integer")
                        .HasColumnName("mileage");

                    b.Property<string>("ModelName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("model");

                    b.Property<string>("Preview")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("preview");

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

                    b.HasIndex("CreatedAt");

                    b.HasIndex("PublisherId");

                    b.ToTable("listings");
                });

            modelBuilder.Entity("FafCarsApi.Models.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TIMESTAMP(0) WITHOUT TIME ZONE")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("TIMESTAMP(0) WITHOUT TIME ZONE")
                        .HasColumnName("deleted_at");

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

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("phone_number");

                    b.Property<int[]>("Roles")
                        .IsRequired()
                        .HasColumnType("integer[]")
                        .HasColumnName("roles");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TIMESTAMP(0) WITHOUT TIME ZONE")
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.HasIndex("CreatedAt");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("cdb7604f-ddda-439c-8139-bffda01a8580"),
                            CreatedAt = new DateTime(2024, 5, 24, 13, 14, 10, 761, DateTimeKind.Local).AddTicks(9051),
                            Email = "admin@gmail.com",
                            Password = "$2a$13$.d1SFHqpBvxOHW9U7hF2jOnNoa/8/Ya5vn55qbEcU6dgO.QSQ.FTK",
                            PhoneNumber = "+37378000111",
                            Roles = new[] { 0 },
                            UpdatedAt = new DateTime(2024, 5, 24, 13, 14, 10, 771, DateTimeKind.Local).AddTicks(3371),
                            Username = "admin"
                        },
                        new
                        {
                            Id = new Guid("5df812c8-d8be-4a9f-92f3-0cc5b3b78a1d"),
                            CreatedAt = new DateTime(2024, 5, 24, 13, 14, 11, 302, DateTimeKind.Local).AddTicks(6870),
                            Email = "user@gmail.com",
                            Password = "$2a$13$S0rwoJ3JwL0VqumRBbct/OqWqHkJE3euNVp1zPUVROto/rIQD7jjW",
                            PhoneNumber = "+37378111222",
                            Roles = new[] { 0 },
                            UpdatedAt = new DateTime(2024, 5, 24, 13, 14, 11, 302, DateTimeKind.Local).AddTicks(6940),
                            Username = "user"
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
