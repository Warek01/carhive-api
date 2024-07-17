using FafCarsApi.Data;
using FafCarsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FafCarsApi.Services;

public class CityService(FafCarsDbContext dbContext) {
  public async Task<List<string>> FindCities(string countryCode, string search) {
    return await dbContext.Cities
      .AsNoTracking()
      .Where(c => c.CountryCode == countryCode)
      .OrderByDescending(c => EF.Functions.TrigramsWordSimilarity(c.Name, search))
      .Take(10)
      .Select(c => c.Name)
      .ToListAsync();
  }
}
