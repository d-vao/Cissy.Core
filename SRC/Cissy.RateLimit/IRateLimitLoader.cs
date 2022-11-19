using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cissy.Caching.Redis;

namespace Cissy.RateLimit
{
    public interface IRateLimitLoader
    {
        IRedisCache GetRedisCache();
        IpRateLimitOptions GetIpRateLimitOptions();
        IpRateLimitPolicies GetIpRateLimitPolicies();
        ClientRateLimitOptions GetClientRateLimitOptions();
        ClientRateLimitPolicies GetClientRateLimitPolicies();

    }
}
