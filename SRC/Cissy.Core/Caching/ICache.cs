using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace Cissy.Caching
{

    public interface ICache
    {
        Task<bool> ContainsKeyAsync<T>(CacheKey key) where T : class, IModel;
        Task<T> GetAsync<T>(CacheKey key) where T : class, IModel;
        Task<T> GetAsync<T>(CacheKey Key, Func<T> func, TimeSpan duration) where T : class, IModel;
        Task SetAsync<T>(CacheKey Key, T target, TimeSpan duration) where T : class, IModel;
        Task RemoveAsync<T>(CacheKey Key) where T : class, IModel;
    }
    //public interface ICache<T>
    //{
    //    bool ContainsKey(string Key);
    //    T Get(string key);
    //    T Get(string Key, Func<T> func, TimeSpan duration);
    //    void Set(string Key, T target, TimeSpan duration);
    //}
}
