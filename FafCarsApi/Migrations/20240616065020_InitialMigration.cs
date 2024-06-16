using System;
using System.Collections.Generic;
using FafCarsApi.Enums;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

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
                .Annotation("Npgsql:Enum:engine_type", "petrol,diesel,hybrid,electric,other")
                .Annotation("Npgsql:Enum:user_role", "admin,listing_creator");

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Code = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Models",
                columns: table => new
                {
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Models", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "UUID_GENERATE_V4()"),
                    Username = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Roles = table.Column<List<UserRole>>(type: "user_role[]", nullable: false),
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
                name: "Brands",
                columns: table => new
                {
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CountryCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Name);
                    table.ForeignKey(
                        name: "FK_Brands_Countries_CountryCode",
                        column: x => x.CountryCode,
                        principalTable: "Countries",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CountryCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Name);
                    table.ForeignKey(
                        name: "FK_Cities_Countries_CountryCode",
                        column: x => x.CountryCode,
                        principalTable: "Countries",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModelsBrands",
                columns: table => new
                {
                    BrandsName = table.Column<string>(type: "character varying(255)", nullable: false),
                    ModelsName = table.Column<string>(type: "character varying(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelsBrands", x => new { x.BrandsName, x.ModelsName });
                    table.ForeignKey(
                        name: "FK_ModelsBrands_Brands_BrandsName",
                        column: x => x.BrandsName,
                        principalTable: "Brands",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModelsBrands_Models_ModelsName",
                        column: x => x.ModelsName,
                        principalTable: "Models",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Listings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "UUID_GENERATE_V4()"),
                    BrandName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ModelName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
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
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(1) WITHOUT TIME ZONE", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CountryCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    Address = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CityName = table.Column<string>(type: "character varying(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Listings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Listings_Brands_BrandName",
                        column: x => x.BrandName,
                        principalTable: "Brands",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Listings_Cities_CityName",
                        column: x => x.CityName,
                        principalTable: "Cities",
                        principalColumn: "Name");
                    table.ForeignKey(
                        name: "FK_Listings_Countries_CountryCode",
                        column: x => x.CountryCode,
                        principalTable: "Countries",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Listings_Models_ModelName",
                        column: x => x.ModelName,
                        principalTable: "Models",
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

            migrationBuilder.CreateIndex(
                name: "IX_Brands_CountryCode",
                table: "Brands",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CountryCode",
                table: "Cities",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_BrandName",
                table: "Listings",
                column: "BrandName");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_CityName",
                table: "Listings",
                column: "CityName");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_CountryCode",
                table: "Listings",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_CreatedAt",
                table: "Listings",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_ModelName",
                table: "Listings",
                column: "ModelName");

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
                name: "IX_ModelsBrands_ModelsName",
                table: "ModelsBrands",
                column: "ModelsName");

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
                name: "ModelsBrands");

            migrationBuilder.DropTable(
                name: "UsersFavoriteListings");

            migrationBuilder.DropTable(
                name: "Listings");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Models");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
