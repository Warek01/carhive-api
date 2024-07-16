using System.ComponentModel.DataAnnotations;
using FafCarsApi.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FafCarsApi.Queries;

public class ListingsQuery : PaginationQuery {
  /// <summary>
  /// Body styles filter.
  /// </summary>
  [FromQuery(Name = "body")]
  public List<CarBodyStyle>? BodyStyles { get; set; } = null;

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
  public List<string>? BrandNames { get; set; }

  [FromQuery(Name = "engine")]
  public List<CarFuelType>? FuelTypes { get; set; }

  [FromQuery(Name = "priceMin")]
  [Range(0, int.MaxValue)]
  public int? PriceMin { get; set; }

  [FromQuery(Name = "priceMax")]
  [Range(0, int.MaxValue)]
  public int? PriceMax { get; set; }

  [FromQuery(Name = "country")]
  [Length(2, 2)]
  public string? CountryCode { get; set; }

  [FromQuery(Name = "SellAddress")]
  [Length(1, 255)]
  public string? Address { get; set; }
}
