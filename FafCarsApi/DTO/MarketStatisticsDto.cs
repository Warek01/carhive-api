namespace FafCarsApi.Dto;

public class MarketStatisticsDto {
  public int TotalListings { get; set; }

  public int CreatedToday { get; set; }

  public List<CountPerDateDto>? TotalListingsStats { get; set; }

  public List<CountPerDateDto>? CreatedListingsStats { get; set; }
}
