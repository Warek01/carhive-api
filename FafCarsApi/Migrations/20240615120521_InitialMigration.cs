using System;
using System.Collections.Generic;
using FafCarsApi.Enums;
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
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:body_style", "sedan,suv,crossover,van,minivan,hatchback,wagon,coupe,pickup_truck,convertible,other")
                .Annotation("Npgsql:Enum:car_color", "black,white,silver,gray,blue,red,brown,green,beige,yellow,gold,orange,purple,pink,burgundy,turquoise,ivory,bronze,teal,navy")
                .Annotation("Npgsql:Enum:engine_type", "gas,diesel,hybrid,electric,other");

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Name);
                });

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
                    BrandName = table.Column<string>(type: "character varying(255)", nullable: true),
                    ModelName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    BodyStyle = table.Column<BodyStyle>(type: "body_style", nullable: false),
                    Horsepower = table.Column<int>(type: "integer", nullable: true),
                    EngineType = table.Column<EngineType>(type: "engine_type", nullable: false),
                    EngineVolume = table.Column<double>(type: "double precision", nullable: true),
                    Color = table.Column<CarColor>(type: "car_color", nullable: true),
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
                        name: "FK_Listings_Brands_BrandName",
                        column: x => x.BrandName,
                        principalTable: "Brands",
                        principalColumn: "Name");
                    table.ForeignKey(
                        name: "FK_Listings_Users_PublisherId",
                        column: x => x.PublisherId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersFavoriteListings",
                columns: table => new
                {
                    FavoritesId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersFavoritesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersFavoriteListings", x => new { x.FavoritesId, x.UsersFavoritesId });
                    table.ForeignKey(
                        name: "FK_UsersFavoriteListings_Listings_FavoritesId",
                        column: x => x.FavoritesId,
                        principalTable: "Listings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersFavoriteListings_Users_UsersFavoritesId",
                        column: x => x.UsersFavoritesId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Brands",
                column: "Name",
                values: new object[]
                {
                    "Acura",
                    "Alfa Romeo",
                    "Audi",
                    "Bentley",
                    "BMW",
                    "Bugatti",
                    "Buick",
                    "BYD",
                    "Cadillac",
                    "Chery",
                    "Chevrolet",
                    "Chrysler",
                    "Daihatsu",
                    "Dodge",
                    "Ferrari",
                    "Fiat",
                    "Ford",
                    "Geely",
                    "Genesis",
                    "GMC",
                    "Honda",
                    "Hummer",
                    "Hyundai",
                    "Infiniti",
                    "Jaguar",
                    "Jeep",
                    "Kia",
                    "Koenigsegg",
                    "Lada",
                    "Lamborghini",
                    "Land Rover",
                    "Lexus",
                    "Lincoln",
                    "Lotus",
                    "Maserati",
                    "Maybach",
                    "Mazda",
                    "McLaren",
                    "Mercedes-Benz",
                    "Mini",
                    "Mitsubishi",
                    "Nissan",
                    "Oldsmobile",
                    "Pagani",
                    "Pontiac",
                    "Porsche",
                    "Proton",
                    "Ram",
                    "Rolls-Royce",
                    "Saab",
                    "Saturn",
                    "Smart",
                    "Spyker",
                    "Subaru",
                    "Suzuki",
                    "Tesla",
                    "Toyota",
                    "Volkswagen",
                    "Volvo"
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "Email", "Password", "PhoneNumber", "Roles", "UpdatedAt", "Username" },
                values: new object[,]
                {
                    { new Guid("57c6566c-b3fb-495c-a03e-7472029e2e56"), new DateTime(2024, 6, 15, 15, 5, 20, 34, DateTimeKind.Local).AddTicks(7955), null, "admin@gmail.com", "$2a$13$IfjFOJc3.FR9.qnXU2hxWeyUubYzrD2Tr7KYYa4FFR5XQ0AlOn8mG", "+37378000111", new[] { 0, 1 }, new DateTime(2024, 6, 15, 15, 5, 20, 43, DateTimeKind.Local).AddTicks(8683), "admin" },
                    { new Guid("c1e0a155-e3cf-4380-b7d6-ad0c3e5fa511"), new DateTime(2024, 6, 15, 15, 5, 20, 508, DateTimeKind.Local).AddTicks(6957), null, "user@gmail.com", "$2a$13$3TaTDVjNj2CZAH8hODNV6O.W3X3o/75E6vbM5mRx3SV7d3exE/E8y", "+37378111222", new[] { 0 }, new DateTime(2024, 6, 15, 15, 5, 20, 508, DateTimeKind.Local).AddTicks(7012), "user" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Listings_BrandName",
                table: "Listings",
                column: "BrandName");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_CreatedAt",
                table: "Listings",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_Price",
                table: "Listings",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_PublisherId",
                table: "Listings",
                column: "PublisherId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_Year",
                table: "Listings",
                column: "Year");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedAt",
                table: "Users",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsersFavoriteListings_UsersFavoritesId",
                table: "UsersFavoriteListings",
                column: "UsersFavoritesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersFavoriteListings");

            migrationBuilder.DropTable(
                name: "Listings");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
