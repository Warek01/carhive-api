using Api.Data;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

public class CountryService(CarHiveDbContext dbContext) {
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
