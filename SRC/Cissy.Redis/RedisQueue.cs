using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Linq;
using ServiceStack.Redis;
using Newtonsoft.Json;
using Cissy;
using Cissy.Caching;

namespace Cissy.Redis
{
    /// <summary>
    /// Redis队列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RedisQueue<T> where T : class, IModel
    {
        CacheKey _key;
        RedisCluster _redis;
        internal RedisQueue(RedisCluster redis, CacheKey key)
        {
            _redis = redis;
            _key = key;
        }
        public void Enqueue(T item)
        {
            string json = item.ModelToJson();
            _redis.EnqueueItemOnList(_key, json);
        }
        public T Dequeue()
        {

            using (_redis.AcquireLock(new CacheKey() { Module = "sys", Model = "_lock" + _key.ToString() }, TimeSpan.FromSeconds(10)))
            {
                string json = _redis.DequeueItemFromList(_key);
                if (json.IsNotNullAndEmpty())
                {
                    return json.JsonToModel<T>();
                }
                return default(T);
            }
        }
        public string DequeueAsJson()
        {

            using (_redis.AcquireLock(new CacheKey() { Module = "sys", Model = "_lock" + _key.ToString() }, TimeSpan.FromSeconds(10)))
            {
                return _redis.DequeueItemFromList(_key);
            }
        }
        public T First()
        {

            using (_redis.AcquireLock(new CacheKey() { Module = "sys", Model = "_lock" + _key.ToString() }, TimeSpan.FromSeconds(10)))
            {
                try
                {
                    string json = _redis.GetItemFromList(_key, 0);
                    if (json.IsNotNullAndEmpty())
                    {
                        return json.JsonToModel<T>();
                    }
                    return default;
                }
                catch
                {
                    return default;
                }
            }
        }
        public long GetQueueCount()
        {
            return _redis.GetListCount(_key);
        }
    }
}
