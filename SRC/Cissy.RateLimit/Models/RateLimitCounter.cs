using System;

namespace Cissy.RateLimit
{
    /// <summary>
    /// Stores the initial access time and the numbers of calls made from that point
    /// </summary>
    public class RateLimitCounter : IModel
    {
        public DateTime Timestamp { get; set; }

        public double Count { get; set; }
    }
}