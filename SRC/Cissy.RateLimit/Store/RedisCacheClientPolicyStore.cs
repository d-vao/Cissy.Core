using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Cissy.Caching.Redis;

namespace Cissy.RateLimit
{
    public class RedisCacheClientPolicyStore : RedisCacheRateLimitStore<ClientRateLimitPolicy>, IClientPolicyStore
    {
        private readonly ClientRateLimitOptions _options;
        private readonly ClientRateLimitPolicies _policies;

        public RedisCacheClientPolicyStore(
            IRedisCache cache,
           IRateLimitLoader Loader) : base(cache)
        {
            _options = Loader.GetClientRateLimitOptions();
            _policies = Loader.GetClientRateLimitPolicies();
        }

        public async Task SeedAsync()
        {
            // on startup, save the IP rules defined in appsettings
            if (_options != null && _policies?.ClientRules != null)
            {
                foreach (var rule in _policies.ClientRules)
                {
                    await SetAsync($"{_options.ClientPolicyPrefix}_{rule.ClientId}", new ClientRateLimitPolicy { ClientId = rule.ClientId, Rules = rule.Rules }, System.TimeSpan.FromDays(36500)).ConfigureAwait(false);
                }
            }
        }
    }
}