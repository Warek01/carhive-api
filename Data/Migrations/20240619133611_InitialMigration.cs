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
                column: "images_filenames",
                value: new List<string>());

            migrationBuilder.UpdateData(
                table: "listings",
                keyColumn: "id",
                keyValue: new Guid("7f42a1ed-bb14-4e10-9b2c-3b4f80d63f2b"),
                column: "images_filenames",
                value: new List<string>());

            migrationBuilder.UpdateData(
                table: "listings",
                keyColumn: "id",
                keyValue: new Guid("b78f7f22-07af-4dee-8c96-ed34d7b9bb95"),
                column: "images_filenames",
                value: new List<string>());

            migrationBuilder.UpdateData(
                table: "listings",
                keyColumn: "id",
                keyValue: new Guid("dcfe205e-0e10-4a0f-a6b6-4e0ac50e1d9f"),
                column: "images_filenames",
                value: new List<string>());

            migrationBuilder.UpdateData(
                table: "listings",
                keyColumn: "id",
                keyValue: new Guid("f3e6e478-3a21-4b05-9926-6fe29f4a58c0"),
                column: "images_filenames",
                value: new List<string>());

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("29aa0b25-d42a-4877-8b4c-3c359e5bee77"),
                columns: new[] { "password", "roles" },
                values: new object[] { "$2a$13$DOQkkbIVKIhX907XcJjXb.IdJtq9.kBo5IsQoOZiSMuH60sMQnMo6", new List<UserRole> { UserRole.ListingCreator } });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("7e4d9d9b-97d8-4e5c-ad49-abe09837c70c"),
                columns: new[] { "password", "roles" },
                values: new object[] { "$2a$13$AUPs2NVqEI5zRYAbNta0j.jDd1jGU3hAq3xhKkMLTTbFiOALOmFc.", new List<UserRole> { UserRole.ListingCreator } });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("e00e715a-fe5e-4814-b595-6cc3cd316fca"),
                columns: new[] { "password", "roles" },
                values: new object[] { "$2a$13$WRVOUsMRdH8Zi5ZAfypkw.qfM2PL9gfLZGICbHno492xvrnOydoTC", new List<UserRole> { UserRole.Admin } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "listings",
                keyColumn: "id",
                keyValue: new Guid("68f9e03a-30c3-47a7-a2b8-0a7f6a6c0ca1"),
                column: "images_filenames",
                value: new List<string>());

            migrationBuilder.UpdateData(
                table: "listings",
                keyColumn: "id",
                keyValue: new Guid("7f42a1ed-bb14-4e10-9b2c-3b4f80d63f2b"),
                column: "images_filenames",
                value: new List<string>());

            migrationBuilder.UpdateData(
                table: "listings",
                keyColumn: "id",
                keyValue: new Guid("b78f7f22-07af-4dee-8c96-ed34d7b9bb95"),
                column: "images_filenames",
                value: new List<string>());

            migrationBuilder.UpdateData(
                table: "listings",
                keyColumn: "id",
                keyValue: new Guid("dcfe205e-0e10-4a0f-a6b6-4e0ac50e1d9f"),
                column: "images_filenames",
                value: new List<string>());

            migrationBuilder.UpdateData(
                table: "listings",
                keyColumn: "id",
                keyValue: new Guid("f3e6e478-3a21-4b05-9926-6fe29f4a58c0"),
                column: "images_filenames",
                value: new List<string>());

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("29aa0b25-d42a-4877-8b4c-3c359e5bee77"),
                columns: new[] { "password", "roles" },
                values: new object[] { "$2a$13$HbtZTSKCu1DwRl0PwN2TiuNbhatsQSN.ARz.X/9rz4fi6y/74pfhm", new List<UserRole> { UserRole.ListingCreator } });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("7e4d9d9b-97d8-4e5c-ad49-abe09837c70c"),
                columns: new[] { "password", "roles" },
                values: new object[] { "$2a$13$qdbdrQu/b5XIXR4g3BEn9.dhrXDeROO9fIb77WjGqyas2ZNER28m2", new List<UserRole> { UserRole.ListingCreator } });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("e00e715a-fe5e-4814-b595-6cc3cd316fca"),
                columns: new[] { "password", "roles" },
                values: new object[] { "$2a$13$aThntKFXIA9w1n0KJd18TupO1N7e5b4//VvSYVh0aTtCc.ZMGINAC", new List<UserRole> { UserRole.Admin } });
        }
    }
}
