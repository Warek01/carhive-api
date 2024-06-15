using FafCarsApi.Enums;
using FafCarsApi.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace FafCarsApi.Dtos;

public class ListingsQueryDto : PaginationQuery {
  /// <summary>
  /// Body styles filter.
  /// </summary>
  [FromQuery(Name = "body")]
  public IList<BodyStyle>? BodyStyles { get; set; } = null;

  /// <summary>
  /// User ID to get associated listings.
  /// </summary>
  [FromQuery(Name = "user")]
  public Guid? UserId { get; set; } = null;

  /// <summary>
  /// Should return user's favorites. Requires "user" to be set.
  /// </summary>
  [FromQuery(Name = "favorites")]
  public bool Favorites { get; set; } = false;

  [FromQuery(Name = "brand")] public IList<string>? BrandNames { get; set; } = null;
  [FromQuery(Name = "engine")] public IList<EngineType>? EngineTypes { get; set; } = null;
  [FromQuery(Name = "priceMin")] public uint? PriceMin { get; set; } = null;
  [FromQuery(Name = "priceMax")] public uint? PriceMax { get; set; } = null;
}
