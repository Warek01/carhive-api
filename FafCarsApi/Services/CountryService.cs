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
}
