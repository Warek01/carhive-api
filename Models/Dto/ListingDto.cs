using FafCarsApi.Models.Entities;

namespace FafCarsApi.Models.Dto;

public class ListingDto
{
  public Guid Id { get; set; }
  public string BrandName { get; set; } = null!;
  public string ModelName { get; set; } = null!;
  public double Price { get; set; }
  public string Type { get; set; } = null!;
  public int? Horsepower { get; set; }
  public string? EngineType { get; set; }
  public double? EngineVolume { get; set; }
  public string? Color { get; set; }
  public int? Clearance { get; set; }
  public int? WheelSize { get; set; }
  public int? Mileage { get; set; }
  public int? Year { get; set; }
  public UserDto? Publisher { get; set; }

  // From listings -----------------------------------------
  public static ListingDto FromListingWithoutUser(Listing l)
  {
    return new ListingDto
    {
      Id = l.Id,
      BrandName = l.BrandName,
      ModelName = l.ModelName,
      Clearance = l.Clearance,
      Horsepower = l.Horsepower,
      EngineType = l.EngineType,
      EngineVolume = l.EngineVolume,
      WheelSize = l.WheelSize,
      Color = l.Color,
      Mileage = l.Mileage,
      Price = l.Price,
      Type = l.Type,
      Year = l.Year,
    };
  }

  public static ListingDto FromListingWithUser(Listing l)
  {
    return new ListingDto
    {
      Id = l.Id,
      BrandName = l.BrandName,
      ModelName = l.ModelName,
      Clearance = l.Clearance,
      Horsepower = l.Horsepower,
      EngineType = l.EngineType,
      EngineVolume = l.EngineVolume,
      WheelSize = l.WheelSize,
      Color = l.Color,
      Mileage = l.Mileage,
      Price = l.Price,
      Type = l.Type,
      Year = l.Year,
      Publisher = new UserDto
      {
        Id = l.Publisher.Id,
        Username = l.Publisher.Username
      }
    };
  }
}
