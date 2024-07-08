using Microsoft.AspNetCore.Mvc;

namespace FafCarsApi.Queries;

public class MarketStatisticsQuery : StatisticsQuery {
  [FromQuery(Name = "includeStats")]
  public bool IncludeStats { get; set; } = false;
}
