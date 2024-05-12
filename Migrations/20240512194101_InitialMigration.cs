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
                    { new Guid("1c0b82aa-0b4c-42d6-9958-8a5f3e1be9dd"), "denis@gmail.com", "$2a$13$zBl5vvCX/vNBHCmdi/c3CeaXfq1KTo3HoHVRbGb4dv08Xm8V29rlC", new[] { 0, 3 }, "denis" },
                    { new Guid("8a63c737-c41d-445f-a837-3d22f3fb37e5"), "warek@gmail.com", "$2a$13$byO4cqNyS4.13NzIs9Qj8uSNKH6tHC.H.4AKOjMlpZcAZjJ2j4EbK", new[] { 1 }, "warek" }
                });

            migrationBuilder.InsertData(
                table: "listings",
                columns: new[] { "id", "brand", "clearance", "color", "deleted_at", "engine_type", "engine_volume", "horsepower", "mileage", "model", "preview_url", "price", "publisher_id", "type", "wheel_size", "year" },
                values: new object[,]
                {
                    { new Guid("04ed4a6f-57f1-4343-9f54-35af5dca4b33"), "Mercedes-Benz", 180, "#1C1C1C", null, "Diesel", 2.0, 240, 18000, "E-Class", null, 40000.0, new Guid("1c0b82aa-0b4c-42d6-9958-8a5f3e1be9dd"), "Sedan", 18, 2019 },
                    { new Guid("0767edec-135b-4cb7-9236-15ed39a56a83"), "BMW", 210, "#FFFFFF", new DateTime(2024, 5, 12, 22, 41, 1, 40, DateTimeKind.Local).AddTicks(1472), "Petrol", 3.0, 300, 15000, "X5", null, 35000.0, new Guid("8a63c737-c41d-445f-a837-3d22f3fb37e5"), "SUV", 20, 2018 },
                    { new Guid("2fe514a9-ee7e-403d-8808-26bfc01bc7ac"), "Toyota", 170, "#007A5E", new DateTime(2024, 5, 12, 22, 41, 1, 42, DateTimeKind.Local).AddTicks(6203), "Hybrid", 2.5, 208, 10000, "Camry", null, 25000.0, new Guid("8a63c737-c41d-445f-a837-3d22f3fb37e5"), "Sedan", 18, 2020 },
                    { new Guid("5e3198b8-fdbc-4406-952d-0aae19ba8944"), "Ford", 230, "#0000FF", null, "Gasoline", 5.0, 375, 10000, "F-150", null, 42000.0, new Guid("1c0b82aa-0b4c-42d6-9958-8a5f3e1be9dd"), "Truck", 18, 2022 },
                    { new Guid("698dbc2c-77ac-4c50-b129-4a8aa53794a6"), "Honda", 160, "#002366", null, "Gasoline", 1.8, 174, 20000, "Civic", null, 18000.0, new Guid("1c0b82aa-0b4c-42d6-9958-8a5f3e1be9dd"), "Sedan", 16, 2017 },
                    { new Guid("76a8aa62-d3f3-41bc-88d2-72f77450d834"), "BMW", 140, "#000000", null, "Gasoline", 2.0, 255, 18000, "3 Series", null, 35000.0, new Guid("1c0b82aa-0b4c-42d6-9958-8a5f3e1be9dd"), "Sedan", 18, 2020 },
                    { new Guid("89508075-cbe1-42f9-9b2f-50c2f942abc0"), "Tesla", 160, "#FFFFFF", null, "Electric", 0.0, 450, 5000, "Model 3", null, 50000.0, new Guid("1c0b82aa-0b4c-42d6-9958-8a5f3e1be9dd"), "Sedan", 19, 2021 },
                    { new Guid("a75815e3-7609-4dc6-bde0-1a77a65f3aa1"), "Ford", 230, "#FF0000", null, "Gasoline", 3.5, 375, 25000, "F-150", "https://localhost:44391/api/file/car-1.jpg", 30000.0, new Guid("8a63c737-c41d-445f-a837-3d22f3fb37e5"), "Truck", 17, 2019 },
                    { new Guid("e35455bd-6289-418c-b6ee-bb9853ec58ea"), "Honda", 150, "#FFA500", null, "Gasoline", 2.0, 174, 20000, "Civic", null, 22000.0, new Guid("1c0b82aa-0b4c-42d6-9958-8a5f3e1be9dd"), "Sedan", 16, 2019 },
                    { new Guid("f0045511-fe77-4a3d-a0e3-a0c3c2b5371b"), "Chevrolet", 250, "#800000", null, "Gasoline", 5.2999999999999998, 355, 12000, "Silverado", null, 38000.0, new Guid("1c0b82aa-0b4c-42d6-9958-8a5f3e1be9dd"), "Truck", 20, 2021 },
                    { new Guid("f299a5bb-6554-4344-b621-c5d497e3a51c"), "Toyota", 240, "#006400", null, "Gasoline", 3.5, 278, 15000, "Tacoma", null, 34000.0, new Guid("1c0b82aa-0b4c-42d6-9958-8a5f3e1be9dd"), "Truck", 17, 2020 },
                    { new Guid("fbdf77c3-42e5-483c-91e5-21518ffaf6b3"), "Chevrolet", 250, "#800000", null, "Gasoline", 5.2999999999999998, 355, 12000, "Silverado", null, 38000.0, new Guid("1c0b82aa-0b4c-42d6-9958-8a5f3e1be9dd"), "Truck", 20, 2021 }
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
