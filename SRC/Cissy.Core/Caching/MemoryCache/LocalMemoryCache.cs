using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace Cissy.Caching.MemoryCache
{
    public interface ILocalMemoryCache : ICache
    {

    }
    internal class LocalMemoryCache : ILocalMemoryCache
    {
        IMemoryCache _cache = default;
        public LocalMemoryCache()
        {

        }
        public void Init(MemoryCacheOptions option = default)
        {
            if (option.IsNull())
                option = new MemoryCacheOptions()
                {
                    //未来再定
                };
            _cache = new Microsoft.Extensions.Caching.Memory.MemoryCache(option);
        }
        public async Task<bool> ContainsKeyAsync<T>(CacheKey key) where T : class, IModel
        {
            var result = _cache.TryGetValue<T>(key.ToString(), out T v);
            return await Task.FromResult(result);
        }
        public async Task<T> GetAsync<T>(CacheKey key) where T : class, IModel
        {
            if (_cache.TryGetValue<T>(key.ToString(), out T v))
                return await Task.FromResult(v);
            return default;
        }
        public async Task<T> GetAsync<T>(CacheKey Key, Func<T> func, TimeSpan duration) where T : class, IModel
        {
            var entity = await GetAsync<T>(Key);
            if (entity.IsNull())
            {
                entity = func();
                _cache.Set(Key.ToString(), entity, duration);
            }
            return await Task.FromResult(entity);
        }
        public async Task SetAsync<T>(CacheKey Key, T target, TimeSpan duration) where T : class, IModel
        {
            await Task.Run(() =>
            {
                _cache.Set(Key.ToString(), target, duration);
            });
        }
        public async Task RemoveAsync<T>(CacheKey Key) where T : class, IModel
        {
            await Task.Run(() =>
            {
                _cache.Remove(Key.ToString());
            });
        }
    }
}
