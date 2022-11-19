using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Cissy.Configuration;

namespace Cissy.Kafka
{
    /// <summary>
    /// Kafka帮助
    /// </summary>
    public static class KafkaConfigHelper
    {
        /// <summary>
        /// 注入Kafka服务
        /// </summary>
        /// <param name="cissyConfigBuilder"></param>
        /// <returns></returns>
        public static CissyConfigBuilder AddKafkaConfig(this CissyConfigBuilder cissyConfigBuilder)
        {
            ICissyConfig cissyConfig = cissyConfigBuilder.CissyConfig;
            if (cissyConfig.IsNotNull())
            {
                KafkaConfig kafkaConfig = cissyConfig.GetConfig<KafkaConfig>();
                if (kafkaConfig.IsNotNull())
                {
                    DefaultKafkaFactory factory = new DefaultKafkaFactory();
                    factory.Config = kafkaConfig;
                    cissyConfigBuilder.ServiceCollection.AddSingleton(typeof(IKafakaFactory), factory);
                }
            }
            return cissyConfigBuilder;
        }
        /// <summary>
        /// 注入kafka常驻内存服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cissyConfigBuilder"></param>
        /// <returns></returns>
        public static CissyConfigBuilder AddKafkaHostService<T>(this CissyConfigBuilder cissyConfigBuilder) where T : KafkaHostService, new()
        {
            ICissyConfig cissyConfig = cissyConfigBuilder.CissyConfig;
            if (cissyConfig.IsNotNull())
            {
                KafkaConfig kafkaConfig = cissyConfig.GetConfig<KafkaConfig>();
                if (kafkaConfig.IsNotNull())
                {
                    DefaultKafkaFactory factory = new DefaultKafkaFactory();
                    T service = new T();
                    service.InitService(factory, kafkaConfig);
                    cissyConfigBuilder.ServiceCollection.AddSingleton(typeof(IHostedService), service);
                }
            }
            return cissyConfigBuilder;
        }
    }
}
