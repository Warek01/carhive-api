using System.Text.Json;
using System.Text.Json.Serialization;
using FafCarsApi.Data;
using FafCarsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FafCarsApi.Services;

public class CurrencyService(
  FafCarsDbContext dbContext,
  IConfiguration config,
  CacheService cache,
  HttpClient httpClient
) {
  private class ApiResponse {
    public class ApiMeta {
      [JsonPropertyName("last_updated_at")]
      public DateTime LastUpdatedAt { get; set; }
    }

    public class ApiData {
      [JsonPropertyName("code")]
      public string Code { get; set; } = null!;

      [JsonPropertyName("value")]
      public double Value { get; set; }
    }

    [JsonPropertyName("meta")]
    public ApiMeta Meta { get; set; } = null!;

    [JsonPropertyName("data")]
    public Dictionary<string, ApiData> Data { get; set; } = null!;
  }

  public async Task<double?> GetCurrency(string code) {
    var res = (double?)await cache.Db.HashGetAsync("currency", code);

    if (res != null) {
      return res;
    }

    Currency currency = await FetchCurrency();
    var hashTasks = new List<Task>(currency.Data.Count);

    foreach (KeyValuePair<string, double> pair in currency.Data) {
      Task<bool> task = cache.Db.HashSetAsync(CacheService.Keys.Currencies, pair.Key.ToLower(), pair.Value);
      hashTasks.Add(task);
    }

    await Task.WhenAll(hashTasks);
    await cache.Db.KeyExpireAsync(CacheService.Keys.Currencies, currency.Timestamp.Date.AddDays(1) - DateTime.Now);

    return currency.Data.TryGetValue(code, out double value) ? value : null;
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
      Data = apiResponse.Data
        .Select(pair => new KeyValuePair<string, double>(pair.Key, pair.Value.Value))
        .ToDictionary(),
    };

    await dbContext.AddAsync(currency);
    await dbContext.SaveChangesAsync();

    return currency;
  }
}
