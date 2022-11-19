using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cissy.Caching;
using Cissy.Configuration;
using Cissy.WeiXin.Https;
using Cissy.Http;

namespace Cissy.WeiXin
{
    internal partial class DefaultWeiXinMpApi
    {
        /// <summary>
        /// 获取小程序登录凭证
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<Code2SessionResult> Code2SessionAsync(string code)
        {
            var url = $"{WeiXinApiHelper.BaseUrlssl}/sns/jscode2session?appid={this.Config.AppId}&secret={this.Config.AppSecret}&js_code={code}&grant_type=authorization_code";
            Code2SessionResult result = default;
            await Actor.Public.GetAsync(url, m =>
             {
                 result = m.JsonToModel<Code2SessionResult>();
             });
            return await Task.FromResult(result);
        }

    }
}
