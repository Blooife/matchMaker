using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Profile.Infrastructure.Redis.Interfaces;

namespace Profile.Infrastructure.Redis.Implementations;

public class CacheService(IDistributedCache distributedCache) : ICacheService
{
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        string? cachedValue = await distributedCache.GetStringAsync(key, cancellationToken);

        return cachedValue is null ? null : JsonSerializer.Deserialize<T>(cachedValue);
    }
    
    public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class
    {
        string cacheValue = JsonSerializer.Serialize(value);

        await distributedCache.SetStringAsync(key, cacheValue, cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await distributedCache.RemoveAsync(key, cancellationToken);
    }
}