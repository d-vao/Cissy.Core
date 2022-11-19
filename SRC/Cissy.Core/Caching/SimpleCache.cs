using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Cissy;

namespace Cissy.Caching
{
    public interface ICacheEntity<Entity>
    {
        Entity Target { get; }
        DateTime ExpirtTime { get; }
    }
    public class SimpleCache<T> : ICache<T> where T : class
    {
        class CacheEntity<Entity> : ICacheEntity<Entity>
        {
            public Entity Target { get; set; }
            public DateTime ExpirtTime { get; set; }
            public override bool Equals(object obj)
            {
                if (obj is ICacheEntity<Entity>)
                {
                    ICacheEntity<Entity> entity = obj as ICacheEntity<Entity>;
                    return entity.Target.Equals(this.Target);
                }
                return base.Equals(obj);
            }
            public override int GetHashCode()
            {
                return this.Target.GetHashCode();
            }
        }
        Dictionary<string, ICacheEntity<T>> Pool = new Dictionary<string, ICacheEntity<T>>();
        public T Get(string key)
        {
            lock (this)
            {
                CheckCache();
                ICacheEntity<T> obj = default(ICacheEntity<T>);
                if (Pool.TryGetValue(key, out obj))
                {
                    return obj.Target;
                }
                return default(T);
            }
        }
        public IEnumerable<T> Get(Func<ICacheEntity<T>, bool> predicate)
        {
            foreach (ICacheEntity<T> entity in Pool.Values.Where(predicate))
            {
                yield return entity.Target;
            }
        }
        public T Get(string Key, Func<T> func, TimeSpan duration)
        {
            T entity = default(T);
            entity = Get(Key);
            if (entity == default(T))
            {
                entity = func();
                lock (this)
                {
                    ICacheEntity<T> cacheEntity = new CacheEntity<T>()
                    {
                        Target = entity,
                        ExpirtTime = DateTime.UtcNow.Add(duration)
                    };
                    Pool[Key] = cacheEntity;
                }
            }
            return entity;
        }
        public bool ContainsKey(string Key)
        {
            return Pool.ContainsKey(Key);
        }
        public bool Contains(T entity)
        {
            ICacheEntity<T> cacheEntity = new CacheEntity<T>()
            {
                Target = entity,
                ExpirtTime = DateTime.UtcNow.AddSeconds(1)
            };

            return Pool.Values.Contains(cacheEntity);
        }
        public void Set(string Key, T target, TimeSpan duration)
        {
            lock (this)
            {
                CheckCache();
                ICacheEntity<T> cacheEntity = new CacheEntity<T>()
                {
                    Target = target,
                    ExpirtTime = DateTime.UtcNow.Add(duration)
                };
                Pool[Key] = cacheEntity;
            }
        }
        void CheckCache()
        {
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, ICacheEntity<T>> pair in Pool)
            {
                if (pair.Value.ExpirtTime < DateTime.UtcNow)
                {
                    list.Add(pair.Key);
                }
            }
            foreach (string key in list)
            {
                Pool.Remove(key);
            }
        }
    }
}
