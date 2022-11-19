using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cissy;
using Cissy.Caching;
using Cissy.Configuration;
using Cissy.WeiXin.Https;
using Cissy.Http;
namespace Cissy.WeiXin
{
    internal partial class DefaultWeiXinMpApi
    {

        public async Task<string> GetAccessTokenAsync()
        {
            ICache cache = this._cacheBuilder();
            if (cache.IsNull())
                return await FreshAccessToken();
            try
            {
                var token = await cache.GetAsync<AccessToken>(new AccessTokenCacheKey(this.Config.AppId));
                if (token.IsNotNull() && token.access_token.IsNotNullAndEmpty())
                {
                    var t = token.access_token.Trim();
                    if (t.IsNotNullAndEmpty() && t != "{}")
                    {
                        if (token.CreateTime.AddSeconds(token.expires_in) > DateTime.Now)
                            return token.access_token;
                    }
                }
                return await FreshAccessToken();
            }
            catch
            {
                return await FreshAccessToken();
            }

        }
        public async Task<string> FreshAccessToken()
        {
            string url = $"{WeiXinApiHelper.BaseCgiBinssl}/token?grant_type=client_credential&appid={this.Config.AppId}&secret={this.Config.AppSecret}";
            var token = default(AccessToken);
            await Actor.Public.GetAsync(url, async m =>
             {
                 token = m.JsonToModel<AccessToken>();
                 if (token.IsNotNull())
                 {
                     ICache cache = this._cacheBuilder();
                     if (cache.IsNotNull())
                     {
                         token.CreateTime = DateTime.Now;
                         await cache.SetAsync(new AccessTokenCacheKey(this.Config.AppId), token, TimeSpan.FromSeconds(token.expires_in));
                     }
                 }
             });
            return await Task.FromResult(token?.access_token);
        }
    }
}
