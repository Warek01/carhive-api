using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Distributed;

namespace FafCarsApi.Extensions;

public static class DistributedCacheExtensions {
  private static readonly JsonSerializerOptions SerializerOptions = new() {
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = true,
    AllowTrailingCommas = false,
    PropertyNameCaseInsensitive = false,
    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
  };

  public static Task SetAsync<T>(
    this IDistributedCache cache,
    string key,
    T value,
    DistributedCacheEntryOptions options,
    CancellationToken token = default
  ) {
    return cache.SetAsync(key, JsonSerializer.SerializeToUtf8Bytes(value, SerializerOptions), options, token);
  }

  public static async Task<T?> GetAsync<T>(
    this IDistributedCache cache,
    string key,
    CancellationToken cancellationToken = default
  ) where T : class {
    byte[]? bytes = await cache.GetAsync(key, cancellationToken);

    if (bytes == null) {
      return null;
    }

    using var stream = new MemoryStream(bytes);
    return await JsonSerializer.DeserializeAsync<T>(stream, SerializerOptions, cancellationToken);
  }

  public static T? Get<T>(
    this IDistributedCache cache,
    string key
  ) where T : class {
    byte[]? bytes = cache.Get(key);

    if (bytes == null) {
      return null;
    }

    using var stream = new MemoryStream(bytes);
    return JsonSerializer.Deserialize<T>(stream, SerializerOptions);
  }

  public static void Set<T>(
    this IDistributedCache cache,
    string key,
    T value,
    DistributedCacheEntryOptions options
  ) {
    cache.Set(key, JsonSerializer.SerializeToUtf8Bytes(value, SerializerOptions), options);
  }
}
