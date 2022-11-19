using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Cissy.Caching.Redis;

namespace Cissy.RateLimit
{
    public class RedisCacheIpPolicyStore : RedisCacheRateLimitStore<IpRateLimitPolicies>, IIpPolicyStore
    {
        private readonly IpRateLimitOptions _options;
        private readonly IpRateLimitPolicies _policies;

        public RedisCacheIpPolicyStore(
            IRedisCache cache,
           IRateLimitLoader Loader) : base(cache)
        {
            _options = Loader.GetIpRateLimitOptions();
            _policies = Loader.GetIpRateLimitPolicies();
        }

        public async Task SeedAsync()
        {
            // on startup, save the IP rules defined in appsettings
            if (_options != null && _policies != null)
            {
                await SetAsync($"{_options.IpPolicyPrefix}", _policies, TimeSpan.FromDays(36500)).ConfigureAwait(false);
            }
        }
    }
}