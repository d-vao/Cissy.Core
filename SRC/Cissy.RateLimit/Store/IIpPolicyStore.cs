using System.Threading.Tasks;

namespace Cissy.RateLimit
{
    public interface IIpPolicyStore : IRateLimitStore<IpRateLimitPolicies>
    {
        Task SeedAsync();
    }
}