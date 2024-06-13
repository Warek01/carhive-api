using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FafCarsApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "UUID_GENERATE_V4()"),
                    Username = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Roles = table.Column<int[]>(type: "integer[]", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TIMESTAMP(1) WITHOUT TIME ZONE", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(1) WITHOUT TIME ZONE", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(1) WITHOUT TIME ZONE", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    PhoneNumber = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Listings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "UUID_GENERATE_V4()"),
                    BrandName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ModelName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    Type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Horsepower = table.Column<int>(type: "integer", nullable: true),
                    EngineType = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    EngineVolume = table.Column<double>(type: "double precision", nullable: true),
                    Color = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    Clearance = table.Column<int>(type: "integer", nullable: true),
                    WheelSize = table.Column<int>(type: "integer", nullable: true),
                    Mileage = table.Column<int>(type: "integer", nullable: true),
                    Year = table.Column<int>(type: "integer", nullable: true),
                    Preview = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Images = table.Column<List<string>>(type: "text[]", nullable: false),
                    PublisherId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TIMESTAMP(1) WITHOUT TIME ZONE", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(1) WITHOUT TIME ZONE", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(1) WITHOUT TIME ZONE", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Listings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Listings_Users_PublisherId",
                        column: x => x.PublisherId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListingUser",
                columns: table => new
                {
                    FavoritesId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersFavoritesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingUser", x => new { x.FavoritesId, x.UsersFavoritesId });
                    table.ForeignKey(
                        name: "FK_ListingUser_Listings_FavoritesId",
                        column: x => x.FavoritesId,
                        principalTable: "Listings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ListingUser_Users_UsersFavoritesId",
                        column: x => x.UsersFavoritesId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "Email", "Password", "PhoneNumber", "Roles", "UpdatedAt", "Username" },
                values: new object[,]
                {
                    { new Guid("5df812c8-d8be-4a9f-92f3-0cc5b3b78a1d"), new DateTime(2024, 6, 13, 22, 15, 11, 257, DateTimeKind.Local).AddTicks(6514), null, "user@gmail.com", "$2a$13$9vlKSuKKmE70FLa/5ujDA.U1l.QJ2N3sEPQKzpRHlvs31UWg4nSxa", "+37378111222", new[] { 0 }, new DateTime(2024, 6, 13, 22, 15, 11, 257, DateTimeKind.Local).AddTicks(6611), "user" },
                    { new Guid("cdb7604f-ddda-439c-8139-bffda01a8580"), new DateTime(2024, 6, 13, 22, 15, 10, 780, DateTimeKind.Local).AddTicks(8263), null, "admin@gmail.com", "$2a$13$1jNFjOjBWBfLptFL.YsttecglR2sMK5.iKpyV8gefJwQKncv6vNrW", "+37378000111", new[] { 0, 1 }, new DateTime(2024, 6, 13, 22, 15, 10, 791, DateTimeKind.Local).AddTicks(1062), "admin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Listings_CreatedAt",
                table: "Listings",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_PublisherId",
                table: "Listings",
                column: "PublisherId");

            migrationBuilder.CreateIndex(
                name: "IX_ListingUser_UsersFavoritesId",
                table: "ListingUser",
                column: "UsersFavoritesId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedAt",
                table: "Users",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListingUser");

            migrationBuilder.DropTable(
                name: "Listings");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
