using FafCarsApi.Data;
using FafCarsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FafCarsApi.Services;

public class CountryService(FafCarsDbContext dbContext) {
  public async Task<List<Country>> GetSupportedCountries() {
    return await dbContext.Countries
      .AsNoTracking()
      .Where(c => c.IsSupported == true)
      .ToListAsync();
  }

  public async Task<int> GetSupportedCountriesCount() {
    return await dbContext.Countries
      .AsNoTracking()
      .Where(c => c.IsSupported == true)
      .CountAsync();
  }
}
