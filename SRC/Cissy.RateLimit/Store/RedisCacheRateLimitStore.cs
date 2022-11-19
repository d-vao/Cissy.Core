using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using Cissy.Caching.Redis;
using Cissy.Caching;
namespace Cissy.RateLimit
{
    public class RateLimitCacheKey : CacheKey
    {
        public RateLimitCacheKey(string Id)
        {
            this.Module = "_cissy";
            this.Model = "ratelimit";
            this.TargetName = Id;
        }
    }
    public class RedisCacheRateLimitStore<T> : IRateLimitStore<T> where T : class, IModel
    {
        private readonly IRedisCache _cache;

        public RedisCacheRateLimitStore(IRedisCache cache)
        {
            _cache = cache;
        }

        public Task SetAsync(string id, T entry, TimeSpan expirationTime)
        {
            return _cache.SetAsync(new RateLimitCacheKey(id), entry, expirationTime);
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await _cache.ContainsKeyAsync<T>(new RateLimitCacheKey(id));
        }

        public async Task<T> GetAsync(string id)
        {
            return await _cache.GetAsync<T>(new RateLimitCacheKey(id));
        }

        public Task RemoveAsync(string id)
        {
            return _cache.RemoveAsync<T>(new RateLimitCacheKey(id));
        }
    }
}