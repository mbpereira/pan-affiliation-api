using Newtonsoft.Json;
using Pan.Affiliation.Domain.Caching;
using StackExchange.Redis;

namespace Pan.Affiliation.Infrastructure.Caching;

public class RedisCacheProvider : ICacheProvider
{
    public static class Constants
    {
        public const string SettingsKey = "RedisSettings";
    }

    private readonly IConnectionMultiplexer _multiplexer;
    private readonly IDatabase _database;

    public RedisCacheProvider(IConnectionMultiplexer multiplexer)
    {
        _multiplexer = multiplexer;
        _database = multiplexer.GetDatabase();
    }

    public Task<bool> SaveAsync<T>(string key, T data, TimeSpan expiresAfter)
    {
        return _database.StringSetAsync(
            new RedisKey(key),
            new RedisValue(JsonConvert.SerializeObject(data)),
            expiry: expiresAfter);
    }

    public Task<long> SaveManyAsync<T>(string key, IEnumerable<T> data)
        => _database.SetAddAsync(
            new RedisKey(key),
            data.Select(d => new RedisValue(JsonConvert.SerializeObject(d))).ToArray()
        );

    public Task<bool> RemoveAsync<T>(string key)
        => _database.KeyDeleteAsync(new RedisKey(key));

    public async Task<T?> GetAsync<T>(string key)
    {
        var content = await _database.StringGetAsync(new RedisKey(key));
        
        if (string.IsNullOrEmpty(content))
            return default;
        
        return JsonConvert.DeserializeObject<T?>(content!);
    }

    public async Task<IEnumerable<T>?> GetManyAsync<T>(string key)
    {
        var content = await _database.SetMembersAsync(new RedisKey(key));

        if (content is null || !content.Any())
            return null;

        return content.Select(c => JsonConvert.DeserializeObject<T>(content!.ToString()!))!;
    }
}