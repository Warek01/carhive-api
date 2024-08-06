using Api.Dtos.Response;
using Api.Models;
using Api.Queries;
using Api.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

public class StatisticsService(ListingService listingService) {
  public async Task<MarketStatisticsDto> GenerateMarketStatistics(MarketStatisticsQuery query) {
    var statsDto = new MarketStatisticsDto {
      TotalListings = await listingService
        .GetTotalListings()
        .CountAsync(),
      CreatedToday = await listingService
        .GetTotalListings()
        .Where(l => l.CreatedAt.Date == DateTime.Today)
        .CountAsync()
    };

    if (!query.IncludeStats) {
      return statsDto;
    }

    int year = query.Year!.Value;
    int month = query.Month!.Value;
    int totalDays = DateTime.DaysInMonth(year, month);
    statsDto.CreatedListingsStats = new List<int>();
    statsDto.TotalListingsStats = new List<int>();
    int total = 0;

    List<Listing> listings = await listingService.GetTotalListings()
      .Where(l => l.CreatedAt.Year == year && l.CreatedAt.Month == month)
      .ToListAsync();

    Dictionary<int, int> dailyCounts = listings
      .GroupBy(l => l.CreatedAt.Day)
      .ToDictionary(g => g.Key, g => g.Count());

    for (int day = 1; day <= totalDays; day++) {
      int count = dailyCounts.TryGetValue(day, out var dailyCount) ? dailyCount : 0;
      total += count;

      statsDto.CreatedListingsStats.Add(count);
      statsDto.TotalListingsStats.Add(total);
    }
    
    return statsDto;
  }
}
