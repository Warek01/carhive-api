using FafCarsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FafCarsApi.Services;

public class CountryService(FafCarsDbContext dbContext) {
  public Task<List<Country>> GetCountries() {
    return dbContext.Countries
      .AsNoTracking()
      .ToListAsync();
  }

  public async Task<List<Brand>> GetCountryBrands(string code) {
    Country country = await dbContext.Countries
      .AsNoTracking()
      .Include(c => c.Brands)
      .Where(c => c.Code.ToLower() == code.ToLower())
      .FirstAsync();

    return country.Brands;
  }
}
