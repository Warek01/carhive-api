namespace FafCarsApi.Dto;

public class MarketStatisticsDto {
  public int TotalListings { get; set; }

  public int CreatedToday { get; set; }

  public List<int>? TotalListingsStats { get; set; }

  public List<int>? CreatedListingsStats { get; set; }
}
