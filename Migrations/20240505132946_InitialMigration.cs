using System;
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
                    password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
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
                    publisher_id = table.Column<Guid>(type: "uuid", nullable: false)
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
                columns: new[] { "id", "password", "username" },
                values: new object[,]
                {
                    { new Guid("056178af-552b-48d0-9eb2-2d48cacfd30d"), "warek", "warek" },
                    { new Guid("fb5b1560-f545-41fe-b735-f92ed459e8be"), "denis", "denis" }
                });

            migrationBuilder.InsertData(
                table: "listings",
                columns: new[] { "id", "brand", "clearance", "color", "engine_type", "engine_volume", "horsepower", "mileage", "model", "price", "publisher_id", "type", "wheel_size", "year" },
                values: new object[,]
                {
                    { new Guid("04b21f12-8af0-4439-bf8c-a3ccdb813a5c"), "Ford", 230, "#FF0000", "Gasoline", 3.5, 375, 25000, "F-150", 30000.0, new Guid("056178af-552b-48d0-9eb2-2d48cacfd30d"), "Truck", 17, 2019 },
                    { new Guid("1be84b58-187e-4147-ac71-fca3874cbac6"), "Chevrolet", 250, "#800000", "Gasoline", 5.2999999999999998, 355, 12000, "Silverado", 38000.0, new Guid("056178af-552b-48d0-9eb2-2d48cacfd30d"), "Truck", 20, 2021 },
                    { new Guid("422bc1b8-fc32-4d39-acaa-a691c493084d"), "BMW", 210, "#FFFFFF", "Petrol", 3.0, 300, 15000, "X5", 35000.0, new Guid("056178af-552b-48d0-9eb2-2d48cacfd30d"), "SUV", 20, 2018 },
                    { new Guid("5f67f9aa-96a6-42f9-8cf6-0b2fa772e991"), "Mercedes-Benz", 180, "#1C1C1C", "Diesel", 2.0, 240, 18000, "E-Class", 40000.0, new Guid("056178af-552b-48d0-9eb2-2d48cacfd30d"), "Sedan", 18, 2019 },
                    { new Guid("6002f869-1c13-4295-9f09-03dba361c7cf"), "Toyota", 170, "#007A5E", "Hybrid", 2.5, 208, 10000, "Camry", 25000.0, new Guid("056178af-552b-48d0-9eb2-2d48cacfd30d"), "Sedan", 18, 2020 },
                    { new Guid("d9cdcd3d-58d6-4931-8ca9-ff80c7a0c7bd"), "Honda", 160, "#002366", "Gasoline", 1.8, 174, 20000, "Civic", 18000.0, new Guid("056178af-552b-48d0-9eb2-2d48cacfd30d"), "Sedan", 16, 2017 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_listings_publisher_id",
                table: "listings",
                column: "publisher_id");
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
