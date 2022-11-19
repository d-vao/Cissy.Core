using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Cissy.Configuration;

namespace Cissy.Http
{
    /// <summary>
    /// http代理配置帮助
    /// </summary>
    public static class HttpProxyConfigHelper
    {
        /// <summary>
        /// 注入Http代理服务
        /// </summary>
        /// <param name="cissyConfigBuilder"></param>
        /// <returns></returns>
        public static CissyConfigBuilder AddHttpProxyConfig(this CissyConfigBuilder cissyConfigBuilder)
        {
            ICissyConfig cissyConfig = cissyConfigBuilder.CissyConfig;
            if (cissyConfig.IsNotNull())
            {
                HttpProxyConfig httpProxyConfig = cissyConfig.GetConfig<HttpProxyConfig>();
                if (httpProxyConfig.IsNotNull())
                {
                    DefaultHttpProxyFactory factory = new DefaultHttpProxyFactory();
                    factory.Config = httpProxyConfig;
                    HttpHelper.UseProxy = true;
                    cissyConfigBuilder.ServiceCollection.AddSingleton(typeof(IHttpProxyFactory), factory);
                }
            }
            return cissyConfigBuilder;
        }
    }
}
