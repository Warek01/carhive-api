using FafCarsApi.Data;
using FafCarsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FafCarsApi.Services;

public class CityService(FafCarsDbContext dbContext) {
  public async Task<List<City>> GetAllCities() {
    return await dbContext.Cities
      .AsNoTracking()
      .ToListAsync();
  }
}
