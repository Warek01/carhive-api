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
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    roles = table.Column<int[]>(type: "integer[]", nullable: false)
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
                columns: new[] { "id", "email", "password", "roles", "username" },
                values: new object[,]
                {
                    { new Guid("34de19e0-3217-4207-b050-3e9f1eb42644"), "warek@gmail.com", "$2a$13$s7vtn2zvuVyZDCth3tz7leduUraPXr11s83ZsA2hD2Zhx4P5m.xCO", new[] { 1 }, "warek" },
                    { new Guid("5a3680a5-0f06-433e-b6a6-e7509bafca7a"), "denis@gmail.com", "$2a$13$J1YT7U7aGV1CvtwB.ceGmuUnv2okuMHeuBfaHx7fdXP6Nio4T2KOS", new[] { 0, 3 }, "denis" }
                });

            migrationBuilder.InsertData(
                table: "listings",
                columns: new[] { "id", "brand", "clearance", "color", "deleted_at", "engine_type", "engine_volume", "horsepower", "mileage", "model", "preview_url", "price", "publisher_id", "type", "wheel_size", "year" },
                values: new object[,]
                {
                    { new Guid("00180b5a-ffdd-4d93-8ef9-ccb79c668b88"), "Chevrolet", 250, "#800000", null, "Gasoline", 5.2999999999999998, 355, 12000, "Silverado", null, 38000.0, new Guid("5a3680a5-0f06-433e-b6a6-e7509bafca7a"), "Truck", 20, 2021 },
                    { new Guid("08dc46a0-8f69-4376-a822-790eb27855eb"), "Toyota", 240, "#006400", null, "Gasoline", 3.5, 278, 15000, "Tacoma", null, 34000.0, new Guid("5a3680a5-0f06-433e-b6a6-e7509bafca7a"), "Truck", 17, 2020 },
                    { new Guid("11268a9a-ef7e-4e26-abb9-0dc01a24cd9a"), "Ford", 230, "#0000FF", null, "Gasoline", 5.0, 375, 10000, "F-150", null, 42000.0, new Guid("5a3680a5-0f06-433e-b6a6-e7509bafca7a"), "Truck", 18, 2022 },
                    { new Guid("2c73b60c-8a12-41c8-bad3-e44522d73e1b"), "Honda", 150, "#FFA500", null, "Gasoline", 2.0, 174, 20000, "Civic", null, 22000.0, new Guid("5a3680a5-0f06-433e-b6a6-e7509bafca7a"), "Sedan", 16, 2019 },
                    { new Guid("3a1fa808-da94-4cc3-862b-5796675da0d3"), "Toyota", 170, "#007A5E", new DateTime(2024, 5, 11, 16, 8, 25, 154, DateTimeKind.Local).AddTicks(2031), "Hybrid", 2.5, 208, 10000, "Camry", null, 25000.0, new Guid("34de19e0-3217-4207-b050-3e9f1eb42644"), "Sedan", 18, 2020 },
                    { new Guid("40f7b072-806e-4119-8645-365808a47769"), "Chevrolet", 250, "#800000", null, "Gasoline", 5.2999999999999998, 355, 12000, "Silverado", null, 38000.0, new Guid("5a3680a5-0f06-433e-b6a6-e7509bafca7a"), "Truck", 20, 2021 },
                    { new Guid("44749168-6e3a-4128-a611-60f25800a83e"), "Tesla", 160, "#FFFFFF", null, "Electric", 0.0, 450, 5000, "Model 3", null, 50000.0, new Guid("5a3680a5-0f06-433e-b6a6-e7509bafca7a"), "Sedan", 19, 2021 },
                    { new Guid("79e66eeb-0dc6-4154-8367-d29c20c7e97d"), "Ford", 230, "#FF0000", null, "Gasoline", 3.5, 375, 25000, "F-150", "https://localhost:44391/api/file/car-1.jpg", 30000.0, new Guid("34de19e0-3217-4207-b050-3e9f1eb42644"), "Truck", 17, 2019 },
                    { new Guid("8d45f346-1f5a-4bd0-b023-ea29fbacec69"), "BMW", 210, "#FFFFFF", new DateTime(2024, 5, 11, 16, 8, 25, 152, DateTimeKind.Local).AddTicks(6885), "Petrol", 3.0, 300, 15000, "X5", null, 35000.0, new Guid("34de19e0-3217-4207-b050-3e9f1eb42644"), "SUV", 20, 2018 },
                    { new Guid("a5f48973-77f1-4217-981d-fd437a5017ae"), "BMW", 140, "#000000", null, "Gasoline", 2.0, 255, 18000, "3 Series", null, 35000.0, new Guid("5a3680a5-0f06-433e-b6a6-e7509bafca7a"), "Sedan", 18, 2020 },
                    { new Guid("dd797484-1ca2-4a2b-9a27-2641076621de"), "Mercedes-Benz", 180, "#1C1C1C", null, "Diesel", 2.0, 240, 18000, "E-Class", null, 40000.0, new Guid("5a3680a5-0f06-433e-b6a6-e7509bafca7a"), "Sedan", 18, 2019 },
                    { new Guid("e9948aa9-29bf-4399-85be-dc8216e1d399"), "Honda", 160, "#002366", null, "Gasoline", 1.8, 174, 20000, "Civic", null, 18000.0, new Guid("5a3680a5-0f06-433e-b6a6-e7509bafca7a"), "Sedan", 16, 2017 }
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
