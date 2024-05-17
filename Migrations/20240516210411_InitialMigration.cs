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
                    roles = table.Column<int[]>(type: "integer[]", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "TIMESTAMP(0) WITHOUT TIME ZONE", nullable: true),
                    created_at = table.Column<DateTime>(type: "TIMESTAMP(0) WITHOUT TIME ZONE", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TIMESTAMP(0) WITHOUT TIME ZONE", nullable: false)
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
                    preview_file_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
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
                columns: new[] { "id", "created_at", "deleted_at", "email", "password", "roles", "updated_at", "username" },
                values: new object[,]
                {
                    { new Guid("e4dc0371-2331-4689-9dff-3dab740f59c9"), new DateTime(2024, 5, 17, 0, 4, 9, 986, DateTimeKind.Local).AddTicks(5501), null, "warek@gmail.com", "$2a$13$fTX/OXOAybxCuKnoSxcD5eGcW8aD24sRJpEHO2xR6qqCaSIMOBdaq", new[] { 1 }, new DateTime(2024, 5, 17, 0, 4, 9, 989, DateTimeKind.Local).AddTicks(4590), "warek" },
                    { new Guid("fa37b5f1-9239-435b-ad92-d9a5f689f4ee"), new DateTime(2024, 5, 17, 0, 4, 10, 610, DateTimeKind.Local).AddTicks(5179), null, "denis@gmail.com", "$2a$13$VomVLnp2vyMqjeBco3I5We/wJq5m0F4Xu8JqiCFMeDBH6J1kZIOBG", new[] { 0, 3 }, new DateTime(2024, 5, 17, 0, 4, 10, 610, DateTimeKind.Local).AddTicks(5277), "denis" }
                });

            migrationBuilder.InsertData(
                table: "listings",
                columns: new[] { "id", "brand", "clearance", "color", "deleted_at", "engine_type", "engine_volume", "horsepower", "mileage", "model", "preview_file_name", "price", "publisher_id", "type", "wheel_size", "year" },
                values: new object[,]
                {
                    { new Guid("07cd8a32-44c6-4f29-bfd1-f56f933842a7"), "BMW", 210, "#FFFFFF", new DateTime(2024, 5, 17, 0, 4, 10, 613, DateTimeKind.Local).AddTicks(614), "Petrol", 3.0, 300, 15000, "X5", null, 35000.0, new Guid("e4dc0371-2331-4689-9dff-3dab740f59c9"), "SUV", 20, 2018 },
                    { new Guid("0a232e44-7269-4512-a8b9-0523b6a50b66"), "BMW", 140, "#000000", null, "Gasoline", 2.0, 255, 18000, "3 Series", null, 35000.0, new Guid("fa37b5f1-9239-435b-ad92-d9a5f689f4ee"), "Sedan", 18, 2020 },
                    { new Guid("1d205787-f87d-4ea7-821c-7f91d90060d8"), "Toyota", 240, "#006400", null, "Gasoline", 3.5, 278, 15000, "Tacoma", null, 34000.0, new Guid("fa37b5f1-9239-435b-ad92-d9a5f689f4ee"), "Truck", 17, 2020 },
                    { new Guid("36575e04-d26f-4d8d-b316-aa52e4426a70"), "Ford", 230, "#0000FF", null, "Gasoline", 5.0, 375, 10000, "F-150", null, 42000.0, new Guid("fa37b5f1-9239-435b-ad92-d9a5f689f4ee"), "Truck", 18, 2022 },
                    { new Guid("3c34c4cf-d60d-4a52-a7fa-92e9fd28a232"), "Honda", 150, "#FFA500", null, "Gasoline", 2.0, 174, 20000, "Civic", null, 22000.0, new Guid("fa37b5f1-9239-435b-ad92-d9a5f689f4ee"), "Sedan", 16, 2019 },
                    { new Guid("49f0fa8c-e980-4000-b77f-49c97a553086"), "Chevrolet", 250, "#800000", null, "Gasoline", 5.2999999999999998, 355, 12000, "Silverado", null, 38000.0, new Guid("fa37b5f1-9239-435b-ad92-d9a5f689f4ee"), "Truck", 20, 2021 },
                    { new Guid("5fc0b7d3-c2ef-4701-b6bd-bd1fecc77d1b"), "Toyota", 170, "#007A5E", new DateTime(2024, 5, 17, 0, 4, 10, 613, DateTimeKind.Local).AddTicks(1339), "Hybrid", 2.5, 208, 10000, "Camry", null, 25000.0, new Guid("e4dc0371-2331-4689-9dff-3dab740f59c9"), "Sedan", 18, 2020 },
                    { new Guid("67498b2a-1eac-4972-bd2f-f78f32d3af11"), "Mercedes-Benz", 180, "#1C1C1C", null, "Diesel", 2.0, 240, 18000, "E-Class", null, 40000.0, new Guid("fa37b5f1-9239-435b-ad92-d9a5f689f4ee"), "Sedan", 18, 2019 },
                    { new Guid("6a0def42-38de-4568-b0f0-192b1413c611"), "Ford", 230, "#FF0000", null, "Gasoline", 3.5, 375, 25000, "F-150", null, 30000.0, new Guid("e4dc0371-2331-4689-9dff-3dab740f59c9"), "Truck", 17, 2019 },
                    { new Guid("71dde5bc-462c-4e49-91b0-1a5575258067"), "Tesla", 160, "#FFFFFF", null, "Electric", 0.0, 450, 5000, "Model 3", null, 50000.0, new Guid("fa37b5f1-9239-435b-ad92-d9a5f689f4ee"), "Sedan", 19, 2021 },
                    { new Guid("d055aa20-036d-4a05-a6cf-644d421c5b19"), "Chevrolet", 250, "#800000", null, "Gasoline", 5.2999999999999998, 355, 12000, "Silverado", null, 38000.0, new Guid("fa37b5f1-9239-435b-ad92-d9a5f689f4ee"), "Truck", 20, 2021 },
                    { new Guid("fc5a1e8f-f099-4e13-be28-4a25e3a07794"), "Honda", 160, "#002366", null, "Gasoline", 1.8, 174, 20000, "Civic", null, 18000.0, new Guid("fa37b5f1-9239-435b-ad92-d9a5f689f4ee"), "Sedan", 16, 2017 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_listings_publisher_id",
                table: "listings",
                column: "publisher_id");

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
