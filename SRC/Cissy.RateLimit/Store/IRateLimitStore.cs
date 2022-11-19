using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cissy.RateLimit
{
    public interface IRateLimitStore<T> where T : class, IModel
    {
        Task<bool> ExistsAsync(string id);
        Task<T> GetAsync(string id);
        Task RemoveAsync(string id);
        Task SetAsync(string id, T entry, TimeSpan expirationTime);
    }
}