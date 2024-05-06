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
                columns: new[] { "id", "password", "username" },
                values: new object[,]
                {
                    { new Guid("155edea0-aecd-427f-9c33-ef089307b81f"), "test", "test" },
                    { new Guid("5a473516-a69e-4c63-8738-4dcb0b8facbd"), "warek", "warek" },
                    { new Guid("6385173c-ff0c-467a-b973-5e3c1c540270"), "password", "user" },
                    { new Guid("aff7ddad-8f4f-4e72-a20a-5090a3e6eb93"), "denis", "denis" },
                    { new Guid("d709614d-2352-48a5-a179-ada7f26a9f65"), "alex", "alex" }
                });

            migrationBuilder.InsertData(
                table: "listings",
                columns: new[] { "id", "brand", "clearance", "color", "deleted_at", "engine_type", "engine_volume", "horsepower", "mileage", "model", "preview_url", "price", "publisher_id", "type", "wheel_size", "year" },
                values: new object[,]
                {
                    { new Guid("010bf79f-1bad-4c05-a134-1c27c052c455"), "Toyota", 240, "#006400", null, "Gasoline", 3.5, 278, 15000, "Tacoma", null, 34000.0, new Guid("aff7ddad-8f4f-4e72-a20a-5090a3e6eb93"), "Truck", 17, 2020 },
                    { new Guid("2e5d84da-14f1-4983-b32b-abc2a67eb5f1"), "Chevrolet", 250, "#800000", null, "Gasoline", 5.2999999999999998, 355, 12000, "Silverado", null, 38000.0, new Guid("aff7ddad-8f4f-4e72-a20a-5090a3e6eb93"), "Truck", 20, 2021 },
                    { new Guid("2e9e7952-fdcc-4513-bf1d-63895758527e"), "BMW", 210, "#FFFFFF", new DateTime(2024, 5, 6, 11, 3, 19, 769, DateTimeKind.Local).AddTicks(4580), "Petrol", 3.0, 300, 15000, "X5", null, 35000.0, new Guid("5a473516-a69e-4c63-8738-4dcb0b8facbd"), "SUV", 20, 2018 },
                    { new Guid("4d5602c4-699a-4f0f-bd4c-fcca31108773"), "Tesla", 160, "#FFFFFF", null, "Electric", 0.0, 450, 5000, "Model 3", null, 50000.0, new Guid("aff7ddad-8f4f-4e72-a20a-5090a3e6eb93"), "Sedan", 19, 2021 },
                    { new Guid("4fda2bd6-d6ec-4f8c-9976-5e8dcd73d9aa"), "Ford", 230, "#0000FF", null, "Gasoline", 5.0, 375, 10000, "F-150", null, 42000.0, new Guid("aff7ddad-8f4f-4e72-a20a-5090a3e6eb93"), "Truck", 18, 2022 },
                    { new Guid("5f68d3ea-7564-4e27-8ce0-131e83852a8e"), "BMW", 140, "#000000", null, "Gasoline", 2.0, 255, 18000, "3 Series", null, 35000.0, new Guid("aff7ddad-8f4f-4e72-a20a-5090a3e6eb93"), "Sedan", 18, 2020 },
                    { new Guid("5f9a4f6d-0755-42af-8183-e0848c3c0bb2"), "Mercedes-Benz", 180, "#1C1C1C", null, "Diesel", 2.0, 240, 18000, "E-Class", null, 40000.0, new Guid("aff7ddad-8f4f-4e72-a20a-5090a3e6eb93"), "Sedan", 18, 2019 },
                    { new Guid("6a8c01ed-f38d-475e-8200-387ce2a0859f"), "Honda", 160, "#002366", null, "Gasoline", 1.8, 174, 20000, "Civic", null, 18000.0, new Guid("aff7ddad-8f4f-4e72-a20a-5090a3e6eb93"), "Sedan", 16, 2017 },
                    { new Guid("a672397c-869c-4c27-ac88-41cb2cd4b7bd"), "Toyota", 170, "#007A5E", new DateTime(2024, 5, 6, 11, 3, 19, 771, DateTimeKind.Local).AddTicks(4748), "Hybrid", 2.5, 208, 10000, "Camry", null, 25000.0, new Guid("5a473516-a69e-4c63-8738-4dcb0b8facbd"), "Sedan", 18, 2020 },
                    { new Guid("b4d3b071-6a76-440d-aa92-712e553001ef"), "Honda", 150, "#FFA500", null, "Gasoline", 2.0, 174, 20000, "Civic", null, 22000.0, new Guid("aff7ddad-8f4f-4e72-a20a-5090a3e6eb93"), "Sedan", 16, 2019 },
                    { new Guid("b9cd61df-314b-4911-b1fc-d6d6a1da990b"), "Ford", 230, "#FF0000", null, "Gasoline", 3.5, 375, 25000, "F-150", "https://localhost:44391/api/files/car-1.jpg", 30000.0, new Guid("5a473516-a69e-4c63-8738-4dcb0b8facbd"), "Truck", 17, 2019 },
                    { new Guid("c266620e-b20b-4954-8afe-8f4c18dd7fbc"), "Chevrolet", 250, "#800000", null, "Gasoline", 5.2999999999999998, 355, 12000, "Silverado", null, 38000.0, new Guid("aff7ddad-8f4f-4e72-a20a-5090a3e6eb93"), "Truck", 20, 2021 }
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
