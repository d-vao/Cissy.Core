using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cissy.RateLimit
{
    public class ClientRateLimitProcessor : RateLimitProcessor, IRateLimitProcessor
    {
        private readonly ClientRateLimitOptions _options;
        private readonly IRateLimitStore<ClientRateLimitPolicy> _policyStore;

        public ClientRateLimitProcessor(
           ClientRateLimitOptions options,
           IRateLimitCounterStore counterStore,
           IClientPolicyStore policyStore,
           IRateLimitConfiguration config)
        : base(options, counterStore, new ClientCounterKeyBuilder(options), config)
        {
            _options = options;
            _policyStore = policyStore;
        }

        public async Task<IEnumerable<RateLimitRule>> GetMatchingRulesAsync(ClientRequestIdentity identity)
        {
            var policy = await _policyStore.GetAsync($"{_options.ClientPolicyPrefix}_{identity.ClientId}");

            return GetMatchingRules(identity, policy?.Rules);
        }
    }
}