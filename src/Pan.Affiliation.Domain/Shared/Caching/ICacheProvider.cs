namespace Pan.Affiliation.Domain.Shared.Caching;

public interface ICacheProvider
{
    Task<bool> SaveAsync<T>(string key, T data, TimeSpan expiresAfter);
    Task<long> SaveManyAsync<T>(string key, IEnumerable<T> data);
    Task<bool> RemoveAsync(string key);
    Task<T?> GetAsync<T>(string key);
    Task<IEnumerable<T>?> GetManyAsync<T>(string key);
}