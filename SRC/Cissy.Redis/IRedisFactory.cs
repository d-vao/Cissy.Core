using System;
using System.Collections.Generic;
using System.Text;

namespace Cissy.Redis
{
    public interface IRedisFactory
    {
        RedisConfig Config { get; }
        T BuildRedisCluster<T>(long? Db = null) where T : RedisCluster, new();
    }
}
