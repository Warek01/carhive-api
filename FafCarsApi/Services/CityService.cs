using FafCarsApi.Data;
using FafCarsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FafCarsApi.Services;

public class CityService(FafCarsDbContext dbContext) {
  public async Task<List<string>> FindCities(string countryCode, string? search) {
    IQueryable<City> query = dbContext.Cities
      .AsNoTracking()
      .Where(c => c.CountryCode == countryCode);

    query = search is null or ""
      ? query.OrderBy(c => c.Name)
      : query.OrderByDescending(c => EF.Functions.TrigramsWordSimilarity(c.Name, search));

    query = query.Take(10);
    
    return await query
      .Select(c => c.Name)
      .ToListAsync();
  }

  public async Task<int> GetCitiesCount() {
    return await dbContext.Cities
      .AsNoTracking()
      .CountAsync();
  }
}
