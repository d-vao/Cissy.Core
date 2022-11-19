using System.Threading.Tasks;

namespace Cissy.RateLimit
{
    public interface IClientPolicyStore : IRateLimitStore<ClientRateLimitPolicy>
    {
        Task SeedAsync();
    }
}