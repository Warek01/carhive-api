using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FafCarsApi.Models.Entities;

[Table("listings")]
public class Listing
{
  [Key] [Column("id")] public Guid Id { get; set; }

  [Column("brand")] [StringLength(255)] public string BrandName { get; set; } = null!;

  [Column("model")] [StringLength(255)] public string ModelName { get; set; } = null!;

  [Column("price")]
  [Range(0, double.MaxValue)]
  public double Price { get; set; }

  [Column("type")] [StringLength(255)] public string Type { get; set; } = null!;

  [Column("horsepower")]
  [Range(0, int.MaxValue)]
  public int? Horsepower { get; set; }

  [Column("engine_type")]
  [StringLength(255)]
  public string? EngineType { get; set; }

  [Column("engine_volume")] public double? EngineVolume { get; set; }

  [Column("color")] [StringLength(7)] public string? Color { get; set; } = null!;

  [Column("clearance")]
  [Range(0, int.MaxValue)]
  public int? Clearance { get; set; }

  [Column("wheel_size")]
  [Range(0, int.MaxValue)]
  public int? WheelSize { get; set; }

  [Column("mileage")]
  [Range(0, int.MaxValue)]
  public int? Mileage { get; set; }

  [Column("year")]
  [Range(0, int.MaxValue)]
  public int? Year { get; set; }

  [Column("preview_url")]
  [StringLength(1024)]
  public Uri? PreviewUrl { get; set; }

  [Column("deleted_at", TypeName = "TIMESTAMP(0) WITHOUT TIME ZONE")]
  public DateTime? DeletedAt { get; set; }

  [Column("publisher_id")] public Guid PublisherId { get; set; }

  [Column("created_at", TypeName = "TIMESTAMP(0) WITHOUT TIME ZONE")]
  public DateTime CreatedAt { get; set; }

  [Column("updated_at", TypeName = "TIMESTAMP(0) WITHOUT TIME ZONE")]
  public DateTime UpdatedAt { get; set; }

  public User Publisher { get; set; } = null!;

  public static readonly Listing[] MockListings =
  {
    new Listing
    {
      Id = Guid.NewGuid(),
      BrandName = "BMW",
      ModelName = "X5",
      EngineType = "Petrol",
      Color = "#FFFFFF",
      Year = 2018,
      Clearance = 210,
      Horsepower = 300,
      Mileage = 15_000,
      Type = "SUV",
      EngineVolume = 3.0,
      WheelSize = 20,
      Price = 35_000,
      DeletedAt = DateTime.Now,
      PublisherId = User.MockUsers[0].Id
    },
    new Listing
    {
      Id = Guid.NewGuid(),
      BrandName = "Toyota",
      ModelName = "Camry",
      EngineType = "Hybrid",
      Color = "#007A5E",
      Year = 2020,
      Clearance = 170,
      Horsepower = 208,
      Mileage = 10_000,
      Type = "Sedan",
      EngineVolume = 2.5,
      WheelSize = 18,
      Price = 25_000,
      DeletedAt = DateTime.Now,
      PublisherId = User.MockUsers[0].Id
    },
    new Listing
    {
      Id = Guid.NewGuid(),
      BrandName = "Ford",
      ModelName = "F-150",
      EngineType = "Gasoline",
      Color = "#FF0000",
      Year = 2019,
      Clearance = 230,
      Horsepower = 375,
      Mileage = 25_000,
      Type = "Truck",
      EngineVolume = 3.5,
      WheelSize = 17,
      Price = 30_000,
      PreviewUrl = new Uri("https://localhost:44391/api/file/car-1.jpg"),
      PublisherId = User.MockUsers[0].Id
    },
    new Listing
    {
      Id = Guid.NewGuid(),
      BrandName = "Honda",
      ModelName = "Civic",
      EngineType = "Gasoline",
      Color = "#002366",
      Year = 2017,
      Clearance = 160,
      Horsepower = 174,
      Mileage = 20_000,
      Type = "Sedan",
      EngineVolume = 1.8,
      WheelSize = 16,
      Price = 18_000,
      PublisherId = User.MockUsers[1].Id
    },
    new Listing
    {
      Id = Guid.NewGuid(),
      BrandName = "Mercedes-Benz",
      ModelName = "E-Class",
      EngineType = "Diesel",
      Color = "#1C1C1C",
      Year = 2019,
      Clearance = 180,
      Horsepower = 240,
      Mileage = 18_000,
      Type = "Sedan",
      EngineVolume = 2.0,
      WheelSize = 18,
      Price = 40_000,
      PublisherId = User.MockUsers[1].Id
    },
    new Listing
    {
      Id = Guid.NewGuid(),
      BrandName = "Chevrolet",
      ModelName = "Silverado",
      EngineType = "Gasoline",
      Color = "#800000",
      Year = 2021,
      Clearance = 250,
      Horsepower = 355,
      Mileage = 12_000,
      Type = "Truck",
      EngineVolume = 5.3,
      WheelSize = 20,
      Price = 38_000,
      PublisherId = User.MockUsers[1].Id
    },
    new Listing
    {
      Id = Guid.NewGuid(),
      BrandName = "Chevrolet",
      ModelName = "Silverado",
      EngineType = "Gasoline",
      Color = "#800000",
      Year = 2021,
      Clearance = 250,
      Horsepower = 355,
      Mileage = 12_000,
      Type = "Truck",
      EngineVolume = 5.3,
      WheelSize = 20,
      Price = 38_000,
      PublisherId = User.MockUsers[1].Id
    },
    new Listing
    {
      Id = Guid.NewGuid(),
      BrandName = "Ford",
      ModelName = "F-150",
      EngineType = "Gasoline",
      Color = "#0000FF",
      Year = 2022,
      Clearance = 230,
      Horsepower = 375,
      Mileage = 10_000,
      Type = "Truck",
      EngineVolume = 5.0,
      WheelSize = 18,
      Price = 42_000,
      PublisherId = User.MockUsers[1].Id
    },
    new Listing
    {
      Id = Guid.NewGuid(),
      BrandName = "Toyota",
      ModelName = "Tacoma",
      EngineType = "Gasoline",
      Color = "#006400",
      Year = 2020,
      Clearance = 240,
      Horsepower = 278,
      Mileage = 15_000,
      Type = "Truck",
      EngineVolume = 3.5,
      WheelSize = 17,
      Price = 34_000,
      PublisherId = User.MockUsers[1].Id
    },
    new Listing
    {
      Id = Guid.NewGuid(),
      BrandName = "Honda",
      ModelName = "Civic",
      EngineType = "Gasoline",
      Color = "#FFA500",
      Year = 2019,
      Clearance = 150,
      Horsepower = 174,
      Mileage = 20_000,
      Type = "Sedan",
      EngineVolume = 2.0,
      WheelSize = 16,
      Price = 22_000,
      PublisherId = User.MockUsers[1].Id
    },
    new Listing
    {
      Id = Guid.NewGuid(),
      BrandName = "BMW",
      ModelName = "3 Series",
      EngineType = "Gasoline",
      Color = "#000000",
      Year = 2020,
      Clearance = 140,
      Horsepower = 255,
      Mileage = 18_000,
      Type = "Sedan",
      EngineVolume = 2.0,
      WheelSize = 18,
      Price = 35_000,
      PublisherId = User.MockUsers[1].Id
    },
    new Listing
    {
      Id = Guid.NewGuid(),
      BrandName = "Tesla",
      ModelName = "Model 3",
      EngineType = "Electric",
      Color = "#FFFFFF",
      Year = 2021,
      Clearance = 160,
      Horsepower = 450,
      Mileage = 5_000,
      Type = "Sedan",
      EngineVolume = 0,
      WheelSize = 19,
      Price = 50_000,
      PublisherId = User.MockUsers[1].Id
    },
  };
}
