using System.Collections.Generic;

namespace Cissy.RateLimit
{
    public class IpRateLimitPolicies : IModel
    {
        public List<IpRateLimitPolicy> IpRules { get; set; } = new List<IpRateLimitPolicy>();
    }
}