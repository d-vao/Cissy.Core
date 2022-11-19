using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Cissy.Caching.WeakCaching
{
    public interface IWeakCache : ICache
    {

    }
    /// <summary>
    /// 本地弱引用缓存管理器
    /// </summary>
    public class WeakStorage : IWeakCache
    {
        private readonly ConcurrentDictionary<Key, object> _caches = new ConcurrentDictionary<Key, object>();
        public async Task<bool> ContainsKeyAsync<T>(CacheKey key) where T : class, IModel
        {
            Key CacheKey = new WeakCaching.Key(key.ToString(), typeof(T));
            return await Task.FromResult(_caches.ContainsKey(CacheKey));
        }
        public async Task SetAsync<T>(CacheKey Key, T target, TimeSpan duration) where T : class, IModel
        {
            await Task.Run(() => { Set(Key, target, DateTime.UtcNow.Add(duration)); });
        }
        public void Set<T>(CacheKey Key, T target, DateTime UTCExpirtTime) where T : class, IModel
        {
            Key CacheKey = new WeakCaching.Key(Key.ToString(), typeof(T));
            CacheItem<T> item = new CacheItem<T>(target, UTCExpirtTime);
            _caches[CacheKey] = item;
        }
        public void Delete<T>(CacheKey Key) where T : class, IModel
        {
            Key CacheKey = new WeakCaching.Key(Key.ToString(), typeof(T));
            Object item;
            _caches.TryRemove(CacheKey, out item);
        }
        public async Task<T> GetAsync<T>(CacheKey key) where T : class, IModel
        {
            Key CacheKey = new WeakCaching.Key(key.ToString(), typeof(T));
            if (_caches.TryGetValue(CacheKey, out object obj))
            {
                if (obj is CacheItem<T>)
                {
                    CacheItem<T> citem = obj as CacheItem<T>;
                    if (citem.IsCurrent)
                    {
                        return await Task.FromResult(citem.Target);
                    }
                }
            }
            return default;
        }
        public async Task<T> GetAsync<T>(CacheKey Key, Func<T> func, TimeSpan duration) where T : class, IModel
        {
            return await Get(Key, func, DateTime.UtcNow.Add(duration));
        }
        public async Task<T> Get<T>(CacheKey Key, Func<T> func, DateTime ExpirtTime) where T : class, IModel
        {
            T t = await GetAsync<T>(Key);
            if (t.IsNotNull())
                return t;
            T target = func();
            CacheItem<T> item = new CacheItem<T>(target, ExpirtTime);
            Key CacheKey = new WeakCaching.Key(Key.ToString(), typeof(T));
            _caches[CacheKey] = item;
            return await Task.FromResult(target);
        }
        public bool IsContainKey<T>(CacheKey Key) where T : class, IModel
        {
            Key CacheKey = new WeakCaching.Key(Key.ToString(), typeof(T));
            return _caches.ContainsKey(CacheKey);
        }
        public async Task RemoveAsync<T>(CacheKey Key) where T : class, IModel
        {
            await Task.Run(() =>
            {
                Key CacheKey = new WeakCaching.Key(Key.ToString(), typeof(T));
                _caches.TryRemove(CacheKey, out object v);
            });
        }
    }
}
