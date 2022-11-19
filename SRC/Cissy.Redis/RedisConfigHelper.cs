using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Cissy.Configuration;

namespace Cissy.Redis
{
    /// <summary>
    /// Redis配置帮助
    /// </summary>
    public static class RedisConfigHelper
    {
        /// <summary>
        /// 注入Redis服务
        /// </summary>
        /// <param name="cissyConfigBuilder"></param>
        /// <returns></returns>
        public static CissyConfigBuilder AddRedisConfig(this CissyConfigBuilder cissyConfigBuilder)
        {
            ICissyConfig cissyConfig = cissyConfigBuilder.CissyConfig;
            if (cissyConfig.IsNotNull())
            {
                RedisConfig redisConfig = cissyConfig.GetConfig<RedisConfig>();
                if (redisConfig.IsNotNull())
                {
                    DefaultRedisFactory factory = new DefaultRedisFactory();
                    factory.Config = redisConfig;
                    cissyConfigBuilder.ServiceCollection.AddSingleton(typeof(IRedisFactory), factory);
                }
            }
            return cissyConfigBuilder;
        }
    }
}
