using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Cissy.Configuration;

namespace Cissy.Elasticsearch
{
    /// <summary>
    /// Redis配置帮助
    /// </summary>
    public static class ElasticsearchConfigHelper
    {
        /// <summary>
        /// 注入Redis服务
        /// </summary>
        /// <param name="cissyConfigBuilder"></param>
        /// <returns></returns>
        public static CissyConfigBuilder AddElasticsearchConfig(this CissyConfigBuilder cissyConfigBuilder)
        {
            ICissyConfig cissyConfig = cissyConfigBuilder.CissyConfig;
            if (cissyConfig.IsNotNull())
            {
                ElasticsearchConfig elasticConfig = cissyConfig.GetConfig<ElasticsearchConfig>();
                if (elasticConfig.IsNotNull())
                {
                    DefaultElasticsearchFactory factory = new DefaultElasticsearchFactory();
                    factory.Config = elasticConfig;
                    cissyConfigBuilder.ServiceCollection.AddSingleton(typeof(IElasticsearchFactory), factory);
                }
            }
            return cissyConfigBuilder;
        }
    }
}
