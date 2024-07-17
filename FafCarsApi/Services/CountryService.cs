using FafCarsApi.Data;
using FafCarsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FafCarsApi.Services;

public class CountryService(FafCarsDbContext dbContext) {
  public async Task<List<Country>> GetCountries() {
    return await dbContext.Countries
      .AsNoTracking()
      .ToListAsync();
  }

  public async Task<List<Brand>> GetCountryBrands(string code) {
    Country? country = await dbContext.Countries
      .AsNoTracking()
      .Include(c => c.Brands)
      .Where(c => c.Code.ToLower() == code.ToLower())
      .FirstOrDefaultAsync();

    return country?.Brands ?? [];
  }
}
