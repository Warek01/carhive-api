using FafCarsApi.Data;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace FafCarsApi.Services;

public class CityService(FafCarsDbContext db, CacheService cache) {
  public async Task<List<string>> GetCountryCities(string countryCode) {
    string? citiesStr = await cache.Db.HashGetAsync(CacheService.Keys.Cities, countryCode);

    if (citiesStr != null) {
      return CacheService.Deserialize<List<string>>(citiesStr);
    }

    List<string> cities = await db.Cities
      .AsNoTracking()
      .Where(c => c.CountryCode == countryCode)
      .Select(c => c.Name)
      .ToListAsync();

    await cache.Db.HashSetAsync(
      CacheService.Keys.Cities,
      countryCode,
      CacheService.Serialize(cities),
      flags: CommandFlags.FireAndForget
    );

    return cities;
  }

  public async Task<int> GetCitiesCount() {
    var count = (int?)await cache.Db.HashGetAsync(CacheService.Keys.EntitiesCount, "cities");

    if (count != null) {
      return count.Value;
    }

    count = await db.Cities
      .AsNoTracking()
      .CountAsync();

    await cache.Db.HashSetAsync(
      CacheService.Keys.EntitiesCount,
      "cities",
      count,
      flags: CommandFlags.FireAndForget
    );

    return count.Value;
  }
}
