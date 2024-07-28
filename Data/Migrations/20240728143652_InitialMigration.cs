using System;
using System.Collections.Generic;
using FafCarsApi.Enums;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FafCarsApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "listings",
                keyColumn: "id",
                keyValue: new Guid("68f9e03a-30c3-47a7-a2b8-0a7f6a6c0ca1"),
                column: "images",
                value: new List<string> { "TestImage1.webp", "TestImage2.webp", "TestImage3.webp" });

            migrationBuilder.UpdateData(
                table: "listings",
                keyColumn: "id",
                keyValue: new Guid("7f42a1ed-bb14-4e10-9b2c-3b4f80d63f2b"),
                column: "images",
                value: new List<string> { "TestImage2.webp", "TestImage1.webp", "TestImage3.webp" });

            migrationBuilder.UpdateData(
                table: "listings",
                keyColumn: "id",
                keyValue: new Guid("b78f7f22-07af-4dee-8c96-ed34d7b9bb95"),
                column: "images",
                value: new List<string> { "TestImage1.webp", "TestImage2.webp", "TestImage3.webp" });

            migrationBuilder.UpdateData(
                table: "listings",
                keyColumn: "id",
                keyValue: new Guid("dcfe205e-0e10-4a0f-a6b6-4e0ac50e1d9f"),
                column: "images",
                value: new List<string> { "TestImage3.webp" });

            migrationBuilder.UpdateData(
                table: "listings",
                keyColumn: "id",
                keyValue: new Guid("f3e6e478-3a21-4b05-9926-6fe29f4a58c0"),
                column: "images",
                value: new List<string> { "TestImage2.webp" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("29aa0b25-d42a-4877-8b4c-3c359e5bee77"),
                columns: new[] { "password", "roles" },
                values: new object[] { "$2a$13$DgAMxAEe/vfIeI01BY7Oge2Ku8mW8WzDMs4VHPRxnF11PkRUtYpcS", new List<UserRole> { UserRole.User } });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("7e4d9d9b-97d8-4e5c-ad49-abe09837c70c"),
                columns: new[] { "password", "roles" },
                values: new object[] { "$2a$13$je8dyDKHaozGrqmt27vwo.3MlTMSaINdKsYpjgJkUYIfgng/04.nO", new List<UserRole> { UserRole.User } });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("e00e715a-fe5e-4814-b595-6cc3cd316fca"),
                columns: new[] { "password", "roles" },
                values: new object[] { "$2a$13$gsjekvJ3m68G.67iZA3UjeDHiviE1hgF374qQKAyDT0gzqKdDRBVm", new List<UserRole> { UserRole.SuperAdmin } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "listings",
                keyColumn: "id",
                keyValue: new Guid("68f9e03a-30c3-47a7-a2b8-0a7f6a6c0ca1"),
                column: "images",
                value: new List<string> { "TestImage1.webp", "TestImage2.webp", "TestImage3.webp" });

            migrationBuilder.UpdateData(
                table: "listings",
                keyColumn: "id",
                keyValue: new Guid("7f42a1ed-bb14-4e10-9b2c-3b4f80d63f2b"),
                column: "images",
                value: new List<string> { "TestImage2.webp", "TestImage1.webp", "TestImage3.webp" });

            migrationBuilder.UpdateData(
                table: "listings",
                keyColumn: "id",
                keyValue: new Guid("b78f7f22-07af-4dee-8c96-ed34d7b9bb95"),
                column: "images",
                value: new List<string> { "TestImage1.webp", "TestImage2.webp", "TestImage3.webp" });

            migrationBuilder.UpdateData(
                table: "listings",
                keyColumn: "id",
                keyValue: new Guid("dcfe205e-0e10-4a0f-a6b6-4e0ac50e1d9f"),
                column: "images",
                value: new List<string> { "TestImage3.webp" });

            migrationBuilder.UpdateData(
                table: "listings",
                keyColumn: "id",
                keyValue: new Guid("f3e6e478-3a21-4b05-9926-6fe29f4a58c0"),
                column: "images",
                value: new List<string> { "TestImage2.webp" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("29aa0b25-d42a-4877-8b4c-3c359e5bee77"),
                columns: new[] { "password", "roles" },
                values: new object[] { "$2a$13$bOJY4wIuwnGi/QsNZu3cd.q05eFf2Tx/4elVd13w81jXL9Ti0Prsq", new List<UserRole> { UserRole.User } });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("7e4d9d9b-97d8-4e5c-ad49-abe09837c70c"),
                columns: new[] { "password", "roles" },
                values: new object[] { "$2a$13$nDkwqbIVW26NCvceX.vhgebBh0lNbNw3tm0bN29txJhRPhZTy00JG", new List<UserRole> { UserRole.User } });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("e00e715a-fe5e-4814-b595-6cc3cd316fca"),
                columns: new[] { "password", "roles" },
                values: new object[] { "$2a$13$nxv7pSk/.XjKLa..flY9PeyBRLHZ.13SGYIjod5ImFS/7Y09iXcBO", new List<UserRole> { UserRole.SuperAdmin } });
        }
    }
}
