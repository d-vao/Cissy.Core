using System.Collections.Generic;

namespace Cissy.RateLimit
{
    public class RateLimitPolicy : IModel
    {
        public List<RateLimitRule> Rules { get; set; } = new List<RateLimitRule>();
    }
}
