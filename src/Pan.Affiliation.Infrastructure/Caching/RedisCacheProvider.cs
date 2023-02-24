using Newtonsoft.Json;
using Pan.Affiliation.Domain.Shared.Caching;
using StackExchange.Redis;

namespace Pan.Affiliation.Infrastructure.Caching;

public class RedisCacheProvider : ICacheProvider
{
    private readonly IConnectionMultiplexer _multiplexer;

    public static class Constants
    {
        public const string SettingsKey = "RedisSettings";
    }

    private IDatabase Database => _multiplexer.GetDatabase();

    public RedisCacheProvider(IConnectionMultiplexer multiplexer)
    {
        _multiplexer = multiplexer;
    }

    public Task<bool> SaveAsync<T>(string key, T data, TimeSpan expiresAfter)
    {
        return Database.StringSetAsync(
            new RedisKey(key),
            new RedisValue(JsonConvert.SerializeObject(data)),
            expiry: expiresAfter);
    }

    public Task<long> SaveManyAsync<T>(string key, IEnumerable<T> data)
        => Database.SetAddAsync(
            new RedisKey(key),
            data.Select(d => new RedisValue(JsonConvert.SerializeObject(d))).ToArray()
        );

    public Task<bool> RemoveAsync(string key)
        => Database.KeyDeleteAsync(new RedisKey(key));

    public async Task<T?> GetAsync<T>(string key)
    {
        var content = await Database.StringGetAsync(new RedisKey(key));

        if (string.IsNullOrEmpty(content))
            return default;

        return JsonConvert.DeserializeObject<T?>(content!);
    }

    public async Task<IEnumerable<T>?> GetManyAsync<T>(string key)
    {
        var content = await Database.SetMembersAsync(new RedisKey(key));

        if (!content.Any())
            return null;

        return content.Select(c => JsonConvert.DeserializeObject<T>(c.ToString()))!;
    }
}