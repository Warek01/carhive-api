using System.Text.Json;
using System.Text.Json.Serialization;
using StackExchange.Redis;

namespace FafCarsApi.Services;

public class CacheService(ConnectionMultiplexer muxer) {
  private readonly IDatabase _db = muxer.GetDatabase();

  private static readonly JsonSerializerOptions Options = new() {
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = true,
    AllowTrailingCommas = false,
    PropertyNameCaseInsensitive = false,
    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
  };

  public Task<bool> StringSetAsync<T>(RedisKey key, T value, TimeSpan? expiry = null) where T : class {
    return _db.StringSetAsync(key, JsonSerializer.SerializeToUtf8Bytes(value, Options), expiry);
  }

  public Task<bool> StringSetAsync(RedisKey key, RedisValue value, TimeSpan? expiry = null) {
    return _db.StringSetAsync(key, value, expiry);
  }

  public async Task<T?> StringGetAsync<T>(RedisKey key) where T : class {
    byte[]? bytes = await _db.StringGetAsync(key);

    if (bytes == null) {
      return null;
    }

    using var stream = new MemoryStream(bytes);
    return await JsonSerializer.DeserializeAsync<T>(stream, Options);
  }

  public Task<RedisValue> StringGetAsync(RedisKey key) {
    return _db.StringGetAsync(key);
  }

  public Task<bool> HashSetAsync<T>(RedisKey key, RedisValue hashField, T value)
    where T : class {
    return _db.HashSetAsync(key, hashField, JsonSerializer.SerializeToUtf8Bytes(value, Options));
  }

  public Task<bool> HashSetAsync(RedisKey key, RedisValue hashField, RedisValue value) {
    return _db.HashSetAsync(key, hashField, value);
  }

  public async Task<T?> HashGetAsync<T>(RedisKey key, RedisValue hashField) where T : class {
    byte[]? bytes = await _db.HashGetAsync(key, hashField);

    if (bytes == null) {
      return null;
    }

    using var stream = new MemoryStream(bytes);
    return await JsonSerializer.DeserializeAsync<T>(stream, Options);
  }

  public Task<RedisValue> HashGetAsync(RedisKey key, RedisValue hashField) {
    return _db.HashGetAsync(key, hashField);
  }

  public Task<bool> KeyExpireAsync(RedisKey key, TimeSpan? expiry = null) {
    return _db.KeyExpireAsync(key, expiry);
  }
}
