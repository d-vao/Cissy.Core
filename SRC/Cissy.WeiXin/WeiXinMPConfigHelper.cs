using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Cissy.Configuration;
using Cissy.Caching;

namespace Cissy.WeiXin
{
    /// <summary>
    /// 微信公众号配置帮助
    /// </summary>
    public static class WeiXinMPConfigHelper
    {
        /// <summary>
        /// 注入微信公众号Api服务
        /// </summary>
        /// <param name="cissyConfigBuilder"></param>
        /// <returns></returns>
        public static CissyConfigBuilder AddWeiXinMpApiConfig(this CissyConfigBuilder cissyConfigBuilder, Func<CissyConfigBuilder, ICache> cacheBuilder)
        {
            ICissyConfig cissyConfig = cissyConfigBuilder.CissyConfig;
            if (cissyConfig.IsNotNull())
            {
                AppConfig weiXinMpConfig = cissyConfig.GetConfig<AppConfig>();
                if (weiXinMpConfig.IsNotNull())
                {
                    if (cacheBuilder.IsNull())
                        throw new NullReferenceException($"cacheBuilder参数为空");
                    Func<ICache> func = () =>
                    {
                        return cacheBuilder(cissyConfigBuilder);
                    };
                    var api = new DefaultWeiXinMpApi(weiXinMpConfig, func);
                    cissyConfigBuilder.ServiceCollection.AddSingleton(typeof(IWeiXinMpApi), api);
                }
            }
            return cissyConfigBuilder;
        }
    }
}
