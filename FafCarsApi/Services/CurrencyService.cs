using System.Text.Json;
using System.Text.Json.Serialization;
using FafCarsApi.Data;
using FafCarsApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using FafCarsApi.Extensions;
using FafCarsApi.Helpers;

namespace FafCarsApi.Services;

public class CurrencyService(
  FafCarsDbContext dbContext,
  IConfiguration config,
  IDistributedCache cache,
  HttpClient httpClient
) {
  private class ApiResponse {
    public class ApiMeta {
      [JsonPropertyName("last_updated_at")]
      public DateTime LastUpdatedAt { get; set; }
    }

    [JsonPropertyName("meta")]
    public ApiMeta Meta { get; set; } = null!;

    [JsonPropertyName("data")]
    public Dictionary<string, CurrencyData> Data { get; set; } = null!;
  }

  public async Task<Currency> GetCurrency() {
    var res = await cache.GetAsync<Currency>("currency");

    if (res != null) {
      return res;
    }

    Currency currency = await FetchCurrency();

    await cache.SetAsync(
      "currency",
      currency,
      new DistributedCacheEntryOptions {
        AbsoluteExpiration = currency.Timestamp.Date.AddDays(1)
      }
    );

    return currency;
  }

  private async Task<Currency> FetchCurrency() {
    Currency? currency = await dbContext.Currencies
      .AsNoTracking()
      .Where(c => c.Timestamp.Date == DateTime.Today)
      .FirstOrDefaultAsync();

    if (currency != null) {
      return currency;
    }

    var request = new HttpRequestMessage();
    request.RequestUri = new Uri("https://api.currencyapi.com/v3/latest?base_currency=USD");
    request.Headers.Add("apiKey", config["ApiKey:CurrencyApi"]!);

    HttpResponseMessage res = await httpClient.SendAsync(request);
    Stream rawResult = await res.Content.ReadAsStreamAsync();
    ApiResponse apiResponse = (await JsonSerializer.DeserializeAsync<ApiResponse>(rawResult))!;

    currency = new Currency {
      Timestamp = DateTime.Now,
      Data = apiResponse.Data,
    };

    await dbContext.AddAsync(currency);
    await dbContext.SaveChangesAsync();

    return currency;
  }
}
