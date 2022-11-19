using Microsoft.Extensions.Caching.Distributed;
using Cissy.Caching.Redis;
namespace Cissy.RateLimit
{
    public class RedisCacheRateLimitCounterStore : RedisCacheRateLimitStore<RateLimitCounter>, IRateLimitCounterStore
    {
        public RedisCacheRateLimitCounterStore(IRedisCache cache) : base(cache)
        {
        }
    }
}