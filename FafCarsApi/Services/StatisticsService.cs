using System.Globalization;
using FafCarsApi.Dto;
using FafCarsApi.Queries;
using Microsoft.EntityFrameworkCore;

namespace FafCarsApi.Services;

public class StatisticsService(ListingService listingService) {
  public async Task<MarketStatisticsDto> GenerateMarketStatistics(MarketStatisticsQuery query) {
    var statsDto = new MarketStatisticsDto();

    statsDto.TotalListings = await listingService
      .GetTotalListings()
      .CountAsync();

    statsDto.CreatedToday = await listingService
      .GetTotalListings()
      .Where(l => l.CreatedAt.Date == DateTime.Today)
      .CountAsync();


    if (!query.IncludeStats) {
      return statsDto;
    }

    // will return only dates with nonzero new listings
    List<CountPerDateDto> createdListingsStatsRaw = await listingService
      .GetTotalListings()
      .Where(l => l.CreatedAt.Year == query.Year && l.CreatedAt.Month == query.Month)
      .GroupBy(l => l.CreatedAt.Date)
      .OrderBy(g => g.Key)
      .Select(g => new CountPerDateDto {
        Date = DateOnly.FromDateTime(g.Key).ToString(CultureInfo.CurrentCulture),
        Count = g.Count()
      })
      .ToListAsync();

    int totalDays = DateTime.DaysInMonth(query.Year, query.Month);

    List<CountPerDateDto> createdListingsStats = new List<CountPerDateDto>(totalDays);

    // add missing dates with 0
    for (int day = 1; day <= totalDays; day++) {
      string date = new DateOnly(query.Year, query.Month, day).ToString(CultureInfo.CurrentCulture);
      CountPerDateDto? countOnDate = createdListingsStatsRaw.Find(s => s.Date == date);

      createdListingsStats.Add(countOnDate ?? new CountPerDateDto { Date = date, Count = 0 });
    }

    var totalListingsStats = new List<CountPerDateDto>(totalDays);

    int total = 0;
    foreach (CountPerDateDto createdPerDate in createdListingsStats) {
      total += createdPerDate.Count;
      var totalPerDate = new CountPerDateDto {
        Date = createdPerDate.Date,
        Count = total,
      };
      totalListingsStats.Add(totalPerDate);
    }

    statsDto.CreatedListingsStats = createdListingsStats;
    statsDto.TotalListingsStats = totalListingsStats;

    return statsDto;
  }
}
