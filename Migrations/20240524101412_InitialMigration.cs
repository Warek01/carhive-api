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
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    username = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    password = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    roles = table.Column<int[]>(type: "integer[]", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "TIMESTAMP(0) WITHOUT TIME ZONE", nullable: true),
                    created_at = table.Column<DateTime>(type: "TIMESTAMP(0) WITHOUT TIME ZONE", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "TIMESTAMP(0) WITHOUT TIME ZONE", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    phone_number = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "listings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    brand = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    model = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    price = table.Column<double>(type: "double precision", nullable: false),
                    type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    horsepower = table.Column<int>(type: "integer", nullable: true),
                    engine_type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    engine_volume = table.Column<double>(type: "double precision", nullable: true),
                    color = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    clearance = table.Column<int>(type: "integer", nullable: true),
                    wheel_size = table.Column<int>(type: "integer", nullable: true),
                    mileage = table.Column<int>(type: "integer", nullable: true),
                    year = table.Column<int>(type: "integer", nullable: true),
                    preview = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    images = table.Column<List<string>>(type: "text[]", nullable: false),
                    publisher_id = table.Column<Guid>(type: "uuid", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "TIMESTAMP(0) WITHOUT TIME ZONE", nullable: true),
                    created_at = table.Column<DateTime>(type: "TIMESTAMP(0) WITHOUT TIME ZONE", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "TIMESTAMP(0) WITHOUT TIME ZONE", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_listings", x => x.id);
                    table.ForeignKey(
                        name: "FK_listings_users_publisher_id",
                        column: x => x.publisher_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "created_at", "deleted_at", "email", "password", "phone_number", "roles", "updated_at", "username" },
                values: new object[,]
                {
                    { new Guid("5df812c8-d8be-4a9f-92f3-0cc5b3b78a1d"), new DateTime(2024, 5, 24, 13, 14, 11, 302, DateTimeKind.Local).AddTicks(6870), null, "user@gmail.com", "$2a$13$S0rwoJ3JwL0VqumRBbct/OqWqHkJE3euNVp1zPUVROto/rIQD7jjW", "+37378111222", new[] { 0 }, new DateTime(2024, 5, 24, 13, 14, 11, 302, DateTimeKind.Local).AddTicks(6940), "user" },
                    { new Guid("cdb7604f-ddda-439c-8139-bffda01a8580"), new DateTime(2024, 5, 24, 13, 14, 10, 761, DateTimeKind.Local).AddTicks(9051), null, "admin@gmail.com", "$2a$13$.d1SFHqpBvxOHW9U7hF2jOnNoa/8/Ya5vn55qbEcU6dgO.QSQ.FTK", "+37378000111", new[] { 0 }, new DateTime(2024, 5, 24, 13, 14, 10, 771, DateTimeKind.Local).AddTicks(3371), "admin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_listings_created_at",
                table: "listings",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_listings_publisher_id",
                table: "listings",
                column: "publisher_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_created_at",
                table: "users",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_users_username",
                table: "users",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "listings");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
