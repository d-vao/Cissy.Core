using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Cissy.Configuration;

namespace Cissy.AOS
{
    /// <summary>
    /// Redis配置帮助
    /// </summary>
    public static class AliOSSConfigHelper
    {
        /// <summary>
        /// 注入AliOSS服务
        /// </summary>
        /// <param name="cissyConfigBuilder"></param>
        /// <returns></returns>
        public static CissyConfigBuilder AddAliOSSConfig(this CissyConfigBuilder cissyConfigBuilder)
        {
            ICissyConfig cissyConfig = cissyConfigBuilder.CissyConfig;
            if (cissyConfig.IsNotNull())
            {
                AliOSSConfig aliOssConfig = cissyConfig.GetConfig<AliOSSConfig>();
                if (aliOssConfig.IsNotNull())
                {
                    DefaultAliOSSFactory factory = new DefaultAliOSSFactory();
                    factory.Config = aliOssConfig;
                    cissyConfigBuilder.ServiceCollection.AddSingleton(typeof(IAliOSSFactory), factory);
                }
            }
            return cissyConfigBuilder;
        }
    }
}
