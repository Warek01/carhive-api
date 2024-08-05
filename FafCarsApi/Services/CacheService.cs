using System.Text.Json;
using System.Text.Json.Serialization;
using StackExchange.Redis;

namespace FafCarsApi.Services;

public class CacheService(ConnectionMultiplexer muxer) {
  public readonly IDatabase Db = muxer.GetDatabase();

  public struct Keys {
    public const string Currencies = "currencies";
    public const string RefreshTokens = "refresh-tokens";
    public const string Cities = "cities";
    public const string EntitiesCount = "entities-count";
  }

private static readonly JsonSerializerOptions Options = new() {
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = true,
    AllowTrailingCommas = false,
    PropertyNameCaseInsensitive = false,
    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
  };

  public static ValueTask<T> DeserializeAsync<T>(RedisValue value) where T : class {
    using var stream = new MemoryStream(value!);
    return JsonSerializer.DeserializeAsync<T>(stream, Options)!;
  }

  public static T Deserialize<T>(RedisValue value) where T : class {
    using var stream = new MemoryStream(value!);
    return JsonSerializer.Deserialize<T>(stream, Options)!;
  }

  public static byte[] Serialize<T>(T value) where T : class {
    return JsonSerializer.SerializeToUtf8Bytes(value, Options);
  }
}
