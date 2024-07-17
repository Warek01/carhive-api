using FafCarsApi.Data;
using Microsoft.EntityFrameworkCore;

namespace FafCarsApi.Services;

public class CityService(FafCarsDbContext dbContext) {
  public async Task<List<string>> GetAllCities() {
    return await dbContext.Listings
      .AsNoTracking()
      .Select(l => l.City.ToLower())
      .Distinct()
      .ToListAsync();
  }
}
