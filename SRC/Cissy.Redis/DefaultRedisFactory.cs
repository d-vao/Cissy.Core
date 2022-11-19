using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Cissy.Redis
{
    public sealed class DefaultRedisFactory : IRedisFactory
    {
        RedisCluster cluster = null;
        public RedisConfig Config { get; set; }
        internal DefaultRedisFactory()
        {

        }
        public T BuildRedisCluster<T>(long? Db) where T : RedisCluster, new()
        {
            if (cluster.IsNull())
            {
                cluster = new T();
                if (Db.HasValue)
                {
                    cluster.Db = Db;
                }
                else if (Config.Db.IsNotNullAndEmpty())
                {
                    if (int.TryParse(Config.Db, out int dbnum))
                    {
                        cluster.Db = dbnum;
                    }

                }
                cluster.Init(Config.Name, Config.Connections.ToArray());
            }
            return cluster as T;
        }
    }
}
