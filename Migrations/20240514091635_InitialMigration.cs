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
                    preview_file_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
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
                    { new Guid("10ca3d19-7151-405c-bd76-20957bb92a05"), "warek@gmail.com", "$2a$13$Ds7Nitf/NsuilEtVtM9svePvptmYKeuY821oIo2gv.5yG9ggPxkiq", new[] { 1 }, "warek" },
                    { new Guid("d06cbfa0-6f02-455c-a3cd-a32f1f22b64b"), "denis@gmail.com", "$2a$13$qB8B9Eg30n/SlnImSHL9k.bMecO5ibCaJhqflB4pok5XWJQEuVMAG", new[] { 0, 3 }, "denis" }
                });

            migrationBuilder.InsertData(
                table: "listings",
                columns: new[] { "id", "brand", "clearance", "color", "deleted_at", "engine_type", "engine_volume", "horsepower", "mileage", "model", "preview_file_name", "price", "publisher_id", "type", "wheel_size", "year" },
                values: new object[,]
                {
                    { new Guid("0677eb44-0d0e-4f43-8e01-ecaec5e91fde"), "Tesla", 160, "#FFFFFF", null, "Electric", 0.0, 450, 5000, "Model 3", null, 50000.0, new Guid("d06cbfa0-6f02-455c-a3cd-a32f1f22b64b"), "Sedan", 19, 2021 },
                    { new Guid("0fbb77d0-dd87-472a-9124-978539b5ef30"), "Toyota", 240, "#006400", null, "Gasoline", 3.5, 278, 15000, "Tacoma", null, 34000.0, new Guid("d06cbfa0-6f02-455c-a3cd-a32f1f22b64b"), "Truck", 17, 2020 },
                    { new Guid("299cc158-a59d-44bd-9353-d0137acc56fe"), "Ford", 230, "#FF0000", null, "Gasoline", 3.5, 375, 25000, "F-150", null, 30000.0, new Guid("10ca3d19-7151-405c-bd76-20957bb92a05"), "Truck", 17, 2019 },
                    { new Guid("5bb7b41f-33bc-4fed-9b1b-9c81596b30b8"), "Chevrolet", 250, "#800000", null, "Gasoline", 5.2999999999999998, 355, 12000, "Silverado", null, 38000.0, new Guid("d06cbfa0-6f02-455c-a3cd-a32f1f22b64b"), "Truck", 20, 2021 },
                    { new Guid("6215f7d1-c55d-42e0-8f1b-f2b7e6b3248a"), "Honda", 160, "#002366", null, "Gasoline", 1.8, 174, 20000, "Civic", null, 18000.0, new Guid("d06cbfa0-6f02-455c-a3cd-a32f1f22b64b"), "Sedan", 16, 2017 },
                    { new Guid("68a68b4c-c4e0-4d15-bf37-3559abf10deb"), "Chevrolet", 250, "#800000", null, "Gasoline", 5.2999999999999998, 355, 12000, "Silverado", null, 38000.0, new Guid("d06cbfa0-6f02-455c-a3cd-a32f1f22b64b"), "Truck", 20, 2021 },
                    { new Guid("710c75ad-32c7-40b5-905b-8646d186c328"), "Honda", 150, "#FFA500", null, "Gasoline", 2.0, 174, 20000, "Civic", null, 22000.0, new Guid("d06cbfa0-6f02-455c-a3cd-a32f1f22b64b"), "Sedan", 16, 2019 },
                    { new Guid("90bf64a9-fffc-4e14-ba3d-85f1bf93aa63"), "BMW", 210, "#FFFFFF", new DateTime(2024, 5, 14, 12, 16, 35, 115, DateTimeKind.Local).AddTicks(8278), "Petrol", 3.0, 300, 15000, "X5", null, 35000.0, new Guid("10ca3d19-7151-405c-bd76-20957bb92a05"), "SUV", 20, 2018 },
                    { new Guid("94ed8176-3f94-4a79-bce5-e638fc857bf5"), "Ford", 230, "#0000FF", null, "Gasoline", 5.0, 375, 10000, "F-150", null, 42000.0, new Guid("d06cbfa0-6f02-455c-a3cd-a32f1f22b64b"), "Truck", 18, 2022 },
                    { new Guid("b88c3f76-d8e8-49b9-8878-06eb64dbb3f2"), "Toyota", 170, "#007A5E", new DateTime(2024, 5, 14, 12, 16, 35, 118, DateTimeKind.Local).AddTicks(3264), "Hybrid", 2.5, 208, 10000, "Camry", null, 25000.0, new Guid("10ca3d19-7151-405c-bd76-20957bb92a05"), "Sedan", 18, 2020 },
                    { new Guid("c81d7dff-25e3-43a3-a327-459ad75d1a1e"), "BMW", 140, "#000000", null, "Gasoline", 2.0, 255, 18000, "3 Series", null, 35000.0, new Guid("d06cbfa0-6f02-455c-a3cd-a32f1f22b64b"), "Sedan", 18, 2020 },
                    { new Guid("fc0d2e50-213e-45c0-bf2f-cfe7a1259dba"), "Mercedes-Benz", 180, "#1C1C1C", null, "Diesel", 2.0, 240, 18000, "E-Class", null, 40000.0, new Guid("d06cbfa0-6f02-455c-a3cd-a32f1f22b64b"), "Sedan", 18, 2019 }
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
