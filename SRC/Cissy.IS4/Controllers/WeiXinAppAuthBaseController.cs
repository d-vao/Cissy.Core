using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Cissy.Extensions;
using Cissy.IS4;
using Cissy.IS4.JWT;
using Cissy.Http;
using Cissy.Authentication;
using Cissy.Serialization;
using System.Security.Claims;
using Cissy.Configuration;
using Microsoft.AspNetCore.WebUtilities;
using Cissy.WeiXin;

namespace Cissy.IS4
{
    public abstract class WeiXinAppAuthBaseController : BaseAuthController, IWeiXinAuthController
    {
        public const int RetryCountLimit = 2;
        protected ICissyConfig CissyConfig;
        protected WeiXinMpConfig WeiXinMpConfig;
        protected IWeiXinMpApi WeiXinMpApi;
        protected string ControllerName { get { return this.ControllerContext.ActionDescriptor.ControllerName; } }
        public WeiXinAppAuthBaseController(ICissyConfig cissyConfig, IWeiXinMpApi weiXinMpApi)
        {
            CissyConfig = cissyConfig;
            this.WeiXinMpApi = weiXinMpApi;
            WeiXinMpConfig = CissyConfig.GetConfig<WeiXinMpConfig>();
        }
        public abstract string AuthenticationType { get; }
        public abstract void PreSignInAspect(WeiXinPassport passport, ClaimsPrincipal pricipal);
        [HttpPost]
        public async Task<IActionResult> GetAuthToken(string code, string encryptedData, string iv)
        {
            if (WeiXinMpConfig.IsNull())
                return StatusCode(500);
            var result = WeiXinMpApi.Code2Session(code);
            if (result.IsNull())
                return StatusCode(500);
            var session_key = result.session_key;
            var json = WXEncryptHelper.DecodeEncryptedData(session_key, encryptedData, iv);
            var userInfo = json.JsonToModel<DecodedUserInfo>();
            if (userInfo.unionId.IsNullOrEmpty())
                return StatusCode(406);
            var url = Actor.Public.GetWXAAuthorizeUrl(this.WeiXinMpConfig, userInfo.openId, userInfo.unionId, userInfo.nickName, userInfo.avatarUrl);
            string token = string.Empty;
            if (await Actor.Public.GetAsync(url, m =>
            {
                token = m;
            }))
            {
                DefaultJwtPrincipalBuilder builder = new DefaultJwtPrincipalBuilder(this.WeiXinMpConfig.JwtSecret);
                var claimsPrincipal = builder.GetPrincipal(this.AuthenticationType, token);
                PreSignInAspect(claimsPrincipal.GetWeiXinPassport(), claimsPrincipal);
                DefaultJwtPrincipalBuilder b2 = new DefaultJwtPrincipalBuilder(this.WeiXinMpConfig.JwtSecret);
                return Content(b2.BuildToken(claimsPrincipal.Claims));
            }
            return StatusCode(500);
        }
    }
}
