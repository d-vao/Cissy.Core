using System;
using System.Collections.Generic;
using System.Text;
using Cissy.Caching;
using Cissy.Configuration;

namespace Cissy.WeiXin
{
    internal partial class DefaultWeiXinMpApi : IWeiXinMpApi
    {
        public AppConfig Config { get; internal set; }
        Func<ICache> _cacheBuilder;
        public DefaultWeiXinMpApi(AppConfig config, Func<ICache> cacheBuilder)
        {
            this.Config = config;
            this._cacheBuilder = cacheBuilder;
        }
    }
}
