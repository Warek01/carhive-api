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
                    password = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
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
                    preview_url = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    deleted_at = table.Column<DateTime>(type: "TIMESTAMP(0) WITHOUT TIME ZONE", nullable: true),
                    publisher_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                columns: new[] { "id", "email", "password", "username" },
                values: new object[,]
                {
                    { new Guid("04409af0-8e15-4231-9646-2bb9f394e880"), "denis@gmail.com", "$2a$10$tOnYG8DmuG/Hp/oNWzqbbOEB/u1aBSy1PZ91rtqRHoivmMXfSnkku", "denis" },
                    { new Guid("4079216d-a73a-4b26-834f-b6eeb114d382"), "alex@gmail.com", "$2a$10$Sh1QM9z12l9ueEirhogWe.WjMCx8D81vzszxyWY/UGMzfB4QZF7P6", "alex" },
                    { new Guid("85e6d93e-32c6-4e11-99ec-8e5807f60b37"), "warek@gmail.com", "$2a$10$8Ivr5T4UXqpY7xtPZDZYBu4JtQek1blsl.lPFV8bRXvX1nfFQHDyi", "warek" },
                    { new Guid("8d121497-ade3-4afe-85a0-b4b211ca747e"), "test@gmail.com", "$2a$10$k3oM3Cpp2PTxQFU6oBfhzeAD1GtM7dtVnFMHQRDXhdUgNqWFfF.NC", "test" },
                    { new Guid("f3f2bc14-d5da-4cb9-abde-6b91eb3a8c3e"), "user@gmail.com", "$2a$10$WIikn7Jm9v48owXbq8IMtey9hMH/Avh1oewjljtliOD6vqqxQ9Pmy", "user" }
                });

            migrationBuilder.InsertData(
                table: "listings",
                columns: new[] { "id", "brand", "clearance", "color", "deleted_at", "engine_type", "engine_volume", "horsepower", "mileage", "model", "preview_url", "price", "publisher_id", "type", "wheel_size", "year" },
                values: new object[,]
                {
                    { new Guid("181d9be2-fad3-499f-869b-d66cec2ccce4"), "Tesla", 160, "#FFFFFF", null, "Electric", 0.0, 450, 5000, "Model 3", null, 50000.0, new Guid("04409af0-8e15-4231-9646-2bb9f394e880"), "Sedan", 19, 2021 },
                    { new Guid("18f8b948-dffc-4248-b808-c052c591ed36"), "Ford", 230, "#FF0000", null, "Gasoline", 3.5, 375, 25000, "F-150", "https://localhost:44391/api/file/car-1.jpg", 30000.0, new Guid("85e6d93e-32c6-4e11-99ec-8e5807f60b37"), "Truck", 17, 2019 },
                    { new Guid("2bfcec34-5254-43d8-a82b-46a892323907"), "BMW", 210, "#FFFFFF", new DateTime(2024, 5, 10, 14, 37, 17, 77, DateTimeKind.Local).AddTicks(4361), "Petrol", 3.0, 300, 15000, "X5", null, 35000.0, new Guid("85e6d93e-32c6-4e11-99ec-8e5807f60b37"), "SUV", 20, 2018 },
                    { new Guid("2e3a3455-4e18-48b1-be9d-a0ed60812130"), "Toyota", 170, "#007A5E", new DateTime(2024, 5, 10, 14, 37, 17, 79, DateTimeKind.Local).AddTicks(1606), "Hybrid", 2.5, 208, 10000, "Camry", null, 25000.0, new Guid("85e6d93e-32c6-4e11-99ec-8e5807f60b37"), "Sedan", 18, 2020 },
                    { new Guid("443e5ed0-d7bb-421a-bd29-cccab5e1fee0"), "BMW", 140, "#000000", null, "Gasoline", 2.0, 255, 18000, "3 Series", null, 35000.0, new Guid("04409af0-8e15-4231-9646-2bb9f394e880"), "Sedan", 18, 2020 },
                    { new Guid("4c1b37bc-62b5-4605-aa1a-4f7da6f8f167"), "Ford", 230, "#0000FF", null, "Gasoline", 5.0, 375, 10000, "F-150", null, 42000.0, new Guid("04409af0-8e15-4231-9646-2bb9f394e880"), "Truck", 18, 2022 },
                    { new Guid("69c53d0a-2f1c-4e3a-bb86-0d2c08818226"), "Mercedes-Benz", 180, "#1C1C1C", null, "Diesel", 2.0, 240, 18000, "E-Class", null, 40000.0, new Guid("04409af0-8e15-4231-9646-2bb9f394e880"), "Sedan", 18, 2019 },
                    { new Guid("73edd881-aca3-4abf-9ffb-2a94e0d2f0d3"), "Chevrolet", 250, "#800000", null, "Gasoline", 5.2999999999999998, 355, 12000, "Silverado", null, 38000.0, new Guid("04409af0-8e15-4231-9646-2bb9f394e880"), "Truck", 20, 2021 },
                    { new Guid("804360ef-b268-4bdf-ad3d-3851981c5aab"), "Honda", 150, "#FFA500", null, "Gasoline", 2.0, 174, 20000, "Civic", null, 22000.0, new Guid("04409af0-8e15-4231-9646-2bb9f394e880"), "Sedan", 16, 2019 },
                    { new Guid("bec6bdcc-8e1b-4e84-b61f-0f85ca5ea291"), "Toyota", 240, "#006400", null, "Gasoline", 3.5, 278, 15000, "Tacoma", null, 34000.0, new Guid("04409af0-8e15-4231-9646-2bb9f394e880"), "Truck", 17, 2020 },
                    { new Guid("d186a4fe-ccfc-4cb3-9381-d4062da0824a"), "Honda", 160, "#002366", null, "Gasoline", 1.8, 174, 20000, "Civic", null, 18000.0, new Guid("04409af0-8e15-4231-9646-2bb9f394e880"), "Sedan", 16, 2017 },
                    { new Guid("e59679e5-ff90-40c0-a10c-1e0b6eda8848"), "Chevrolet", 250, "#800000", null, "Gasoline", 5.2999999999999998, 355, 12000, "Silverado", null, 38000.0, new Guid("04409af0-8e15-4231-9646-2bb9f394e880"), "Truck", 20, 2021 }
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
