using System.Text.Json.Serialization;

namespace FafCarsApi.Models;

public class CurrencyData {
  [JsonPropertyName("code")]
  public string Code { get; set; } = null!;

  [JsonPropertyName("value")]
  public double Value { get; set; }
}
