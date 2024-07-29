using System.Text.Json;
using System.Text.Json.Serialization;
using FafCarsApi.Data;
using FafCarsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FafCarsApi.Services;

public class CurrencyService(FafCarsDbContext dbContext, IConfiguration config) {
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

  public async Task<Currency> GetCurrentCurrency() {
    Currency? currency = await dbContext.Currencies
      .AsNoTracking()
      .Where(c => c.Timestamp.Date == DateTime.Today)
      .FirstOrDefaultAsync();

    if (currency != null) {
      return currency;
    }

    var httpClient = new HttpClient();
    var request = new HttpRequestMessage();
    request.RequestUri =
      new Uri(
        $"https://api.currencyapi.com/v3/latest?base_currency={Currency.BaseCurrency}&currencies={Currency.CurrenciesListString}"
      );
    request.Headers.Add("apiKey", config["ApiKey:CurrencyApi"]!);

    HttpResponseMessage res = await httpClient.SendAsync(request);
    Stream stringRes = await res.Content.ReadAsStreamAsync();
    ApiResponse apiResponse = (await JsonSerializer.DeserializeAsync<ApiResponse>(stringRes))!;

    currency = new Currency {
      Timestamp = DateTime.Now,
      Eur = apiResponse.Data["EUR"].Value,
      Mdl = apiResponse.Data["MDL"].Value,
      Ron = apiResponse.Data["RON"].Value,
    };

    await dbContext.AddAsync(currency);
    await dbContext.SaveChangesAsync();

    return currency;
  }
}
