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
    public static class RedisQueueHelper
    {
        public static RedisQueue<T> GetQueue<T>(this RedisCluster redis, CacheKey key) where T : class, IModel
        {
            return new RedisQueue<T>(redis, key);
        }
    }
}
