using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cissy.RateLimit
{
    public interface IRateLimitProcessor
    {
        Task<IEnumerable<RateLimitRule>> GetMatchingRulesAsync(ClientRequestIdentity identity);

        RateLimitHeaders GetRateLimitHeaders(RateLimitCounter? counter, RateLimitRule rule);

        Task<RateLimitCounter> ProcessRequestAsync(ClientRequestIdentity requestIdentity, RateLimitRule rule);

        bool IsWhitelisted(ClientRequestIdentity requestIdentity);
    }
}