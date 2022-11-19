using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cissy.RateLimit
{
    public class MemoryCacheRateLimitStore<T> : IRateLimitStore<T> where T : class, IModel
    {
        private readonly IMemoryCache _cache;

        public MemoryCacheRateLimitStore(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task<bool> ExistsAsync(string id)
        {
            return Task.FromResult(_cache.TryGetValue(id, out _));
        }

        public Task<T> GetAsync(string id)
        {
            if (_cache.TryGetValue(id, out T stored))
            {
                return Task.FromResult(stored);
            }

            return Task.FromResult(default(T));
        }

        public Task RemoveAsync(string id)
        {
            _cache.Remove(id);

            return Task.CompletedTask;
        }

        public Task SetAsync(string id, T entry, TimeSpan expirationTime)
        {
            var options = new MemoryCacheEntryOptions
            {
                Priority = CacheItemPriority.NeverRemove
            };
            options.SetAbsoluteExpiration(expirationTime);
            _cache.Set(id, entry, options);
            return Task.CompletedTask;
        }
    }
}