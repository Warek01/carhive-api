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
                    id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    { new Guid("0c2ff978-4813-4f6a-9da1-f952eb613f83"), "denis", "denis" },
                    { new Guid("c0488d4f-b8b1-47ca-8237-f4f0cc680b5b"), "warek", "warek" }
                });

            migrationBuilder.InsertData(
                table: "listings",
                columns: new[] { "id", "brand", "clearance", "color", "engine_type", "engine_volume", "horsepower", "mileage", "model", "price", "publisher_id", "type", "wheel_size", "year" },
                values: new object[,]
                {
                    { new Guid("5b29c135-2589-4b19-b42d-296f0ff37b5c"), "Honda", 160, "#002366", "Gasoline", 1.8, 174, 20000, "Civic", 18000.0, new Guid("c0488d4f-b8b1-47ca-8237-f4f0cc680b5b"), "Sedan", 16, 2017 },
                    { new Guid("6327cff9-b8fb-49e7-9eb8-ccf0323746b0"), "Mercedes-Benz", 180, "#1C1C1C", "Diesel", 2.0, 240, 18000, "E-Class", 40000.0, new Guid("c0488d4f-b8b1-47ca-8237-f4f0cc680b5b"), "Sedan", 18, 2019 },
                    { new Guid("6423d802-fa57-4921-ad34-62c21cbe12be"), "Toyota", 170, "#007A5E", "Hybrid", 2.5, 208, 10000, "Camry", 25000.0, new Guid("c0488d4f-b8b1-47ca-8237-f4f0cc680b5b"), "Sedan", 18, 2020 },
                    { new Guid("74366ce8-2325-4207-b6e1-69d6f6f1aeec"), "BMW", 210, "#FFFFFF", "Petrol", 3.0, 300, 15000, "X5", 35000.0, new Guid("c0488d4f-b8b1-47ca-8237-f4f0cc680b5b"), "SUV", 20, 2018 },
                    { new Guid("80936498-ed1d-4496-a534-177fc3e352be"), "Ford", 230, "#FF0000", "Gasoline", 3.5, 375, 25000, "F-150", 30000.0, new Guid("c0488d4f-b8b1-47ca-8237-f4f0cc680b5b"), "Truck", 17, 2019 },
                    { new Guid("e6e1cc0c-17bd-4631-bdac-0b4ebfd10f7b"), "Chevrolet", 250, "#800000", "Gasoline", 5.2999999999999998, 355, 12000, "Silverado", 38000.0, new Guid("c0488d4f-b8b1-47ca-8237-f4f0cc680b5b"), "Truck", 20, 2021 }
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
