using System.ComponentModel.DataAnnotations;
using FafCarsApi.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FafCarsApi.Queries;

public class ListingQuery : PaginationQuery {
  /// <summary>
  /// Body styles filter.
  /// </summary>
  [FromQuery(Name = "body")]
  public List<CarBodyStyle> BodyStyles { get; set; } = [];

  /// <summary>
  /// User ID to get associated listings.
  /// </summary>
  [FromQuery(Name = "user")]
  public Guid? UserId { get; set; }

  /// <summary>
  /// Should return user's favorites. Requires "user" to be set.
  /// </summary>
  [FromQuery(Name = "favorites")]
  public bool Favorites { get; set; } = false;

  [FromQuery(Name = "brand")]
  public List<string> BrandNames { get; set; } = [];

  [FromQuery(Name = "model")]
  public List<string> ModelNames { get; set; } = [];

  [FromQuery(Name = "fuelType")]
  public List<CarFuelType> FuelTypes { get; set; } = [];

  [FromQuery(Name = "drivetrain")]
  public List<CarDrivetrain> Drivetrains { get; set; } = [];

  [FromQuery(Name = "transmission")]
  public List<CarTransmission> Transmissions { get; set; } = [];

  [FromQuery(Name = "status")]
  public List<CarStatus> Statuses { get; set; } = [];

  [FromQuery(Name = "color")]
  public List<CarColor> Colors { get; set; } = [];

  [FromQuery(Name = "priceMin")]
  [Range(0, int.MaxValue)]
  public int? PriceMin { get; set; }

  [FromQuery(Name = "priceMax")]
  [Range(0, int.MaxValue)]
  public int? PriceMax { get; set; }

  [FromQuery(Name = "mileageMin")]
  [Range(0, int.MaxValue)]
  public int? MileageMin { get; set; }

  [FromQuery(Name = "mileageMax")]
  [Range(0, int.MaxValue)]
  public int? MileageMax { get; set; }

  [FromQuery(Name = "clearanceMin")]
  [Range(0, int.MaxValue)]
  public int? ClearanceMin { get; set; }

  [FromQuery(Name = "clearanceMax")]
  [Range(0, int.MaxValue)]
  public int? ClearanceMax { get; set; }

  [FromQuery(Name = "wheelSizeMin")]
  [Range(0, int.MaxValue)]
  public int? WheelSizeMin { get; set; }

  [FromQuery(Name = "wheelSizeMax")]
  [Range(0, int.MaxValue)]
  public int? WheelSizeMax { get; set; }

  [FromQuery(Name = "country")]
  [Length(2, 2)]
  public string? CountryCode { get; set; }

  [FromQuery(Name = "city")]
  [Length(1, 255)]
  public string? CityName { get; set; }
}
