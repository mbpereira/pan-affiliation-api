namespace Pan.Affiliation.Domain.Caching;

public interface ICacheProvider
{
    Task<bool> SaveAsync<T>(string key, T data, TimeSpan expiresAfter);
    Task<long> SaveManyAsync<T>(string key, IEnumerable<T> data, TimeSpan expiresAfter);
    Task<bool> RemoveAsync<T>(string key);
    Task<T?> GetAsync<T>(string key);
    Task<IEnumerable<T>?> GetManyAsync<T>(string key);
}