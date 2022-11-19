using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Cissy.RateLimit
{
    public class MemoryCacheIpPolicyStore : MemoryCacheRateLimitStore<IpRateLimitPolicies>, IIpPolicyStore
    {
        private readonly IpRateLimitOptions _options;
        private readonly IpRateLimitPolicies _policies;

        public MemoryCacheIpPolicyStore(
            IMemoryCache cache,
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
                await SetAsync($"{_options.IpPolicyPrefix}", _policies, System.TimeSpan.FromDays(36500)).ConfigureAwait(false);
            }
        }
    }
}