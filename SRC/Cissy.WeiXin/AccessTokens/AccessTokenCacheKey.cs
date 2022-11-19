using System;
using System.Collections.Generic;
using System.Text;
using Cissy.Caching;

namespace Cissy.WeiXin
{
    public class AccessTokenCacheKey : CacheKey
    {
        public AccessTokenCacheKey(string AppId)
        {
            this.Module = "sys";
            this.Model = "weixin";
            this.PropertyName = "accesstoken";
            this.PropertyValue = AppId;
        }
    }
}
