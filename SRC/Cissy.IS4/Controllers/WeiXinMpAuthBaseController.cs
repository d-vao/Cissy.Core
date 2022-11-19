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
    public abstract class WeiXinMpAuthBaseController : BaseAuthController, IWeiXinAuthController
    {
        public const int RetryCountLimit = 2;
        protected ICissyConfig CissyConfig;
        protected WeiXinMpConfig WeiXinMpConfig;
        protected string ControllerName { get { return this.ControllerContext.ActionDescriptor.ControllerName; } }
        public WeiXinMpAuthBaseController(ICissyConfig cissyConfig)
        {
            CissyConfig = cissyConfig;
            WeiXinMpConfig = CissyConfig.GetConfig<WeiXinMpConfig>();
        }
        //public abstract WeiXinMPGrantTypes GrantType { get; }
        public abstract void PreSignInAspect(WeiXinPassport passport, ClaimsPrincipal pricipal);
        [HttpGet]
        public abstract IActionResult DoSignIn(string CookieAuthenticationSchemeScheme, ClaimsPrincipal pricipal);
        [HttpGet]
        [Route("cissy/core/WXLogin/[controller]/SignIn/{AuthenticationScheme}")]
        public IActionResult SignIn(string AuthenticationScheme, string token)
        {
            try
            {
                //var json = new JwtBuilder()
                //    .WithSecret(_WeiXinMpConfig.JwtSecret)
                //    .MustVerifySignature()
                //.Decode(token);
                //return Content(json);
                DefaultJwtPrincipalBuilder builder = new DefaultJwtPrincipalBuilder(WeiXinMpConfig.JwtSecret);
                var claimsPrincipal = builder.GetPrincipal(AuthenticationScheme, token);
                PreSignInAspect(claimsPrincipal.GetWeiXinPassport(), claimsPrincipal);
                return DoSignIn(AuthenticationScheme, claimsPrincipal);
            }
            catch (Exception ep)
            {
                return Redirect("~/");
                //return Content(token + "////" + ep.Message + "///" + ep.StackTrace);
            }
            //return Content("final");
        }
        [HttpGet]
        [Route("cissy/core/WXLogin/[controller]/GetAuth/{AuthenticationScheme}")]
        public IActionResult GetAuth(string AuthenticationScheme)
        {
            if (WeiXinMpConfig.IsNull())
                return Redirect(WeiXinMpConfig.ErrUrl);

            string url = Actor.Public.GetMpAuthorizeUrl(WeiXinMpConfig, this.Request.BaseUrl() + $"/cissy/core/WXLogin/{this.ControllerName}/SignIn/{AuthenticationScheme}");
            //return Content(url);
            return Redirect(url);
        }
        //[Route("cissy/core/WXLogin/[controller]/AuthResponse/{AuthenticationScheme}")]
        //public async Task<IActionResult> AuthResponse(string AuthenticationScheme, string code, string state)
        //{
        //    if (WeiXinMpConfig.IsNull())
        //        return Redirect(WeiXinMpConfig.ErrUrl);
        //    string url = Actor.Public.GetCodeAccessTokenUrl(WeiXinMpConfig.AppId, WeiXinMpConfig.AppSecret, code);
        //    string redirectUrl = WeiXinMpConfig.ErrUrl;
        //    var ok = await Actor.Public.GetJsonAsync<AccessTokenResult>(url, result =>
        //    {
        //        if (result.IsNotNull())
        //        {
        //            redirectUrl = Actor.Public.GetWeiXinSTSUrl(WeiXinMpConfig, result, (WeiXinMpConfig.AuthBaseUrl + $"/cissy/core/WXLogin/{this.ControllerName}/SignIn/{AuthenticationScheme}"), (WeiXinMpConfig.AuthBaseUrl + $"/cissy/core/WXLogin/{this.ControllerName}/GetSeniorAuth/{AuthenticationScheme}"));
        //        }
        //    });
        //    //return Content(redirectUrl);
        //    return Redirect(redirectUrl);
        //}
        //[Route("cissy/core/WXLogin/[controller]/GetSeniorAuth/{AuthenticationScheme}")]
        //public IActionResult GetSeniorAuth(string AuthenticationScheme)
        //{
        //    if (WeiXinMpConfig.IsNull())
        //        return Redirect(WeiXinMpConfig.ErrUrl);
        //    string url = Actor.Public.GetSeniorAuthorizeUrl(WeiXinMpConfig, $"/cissy/core/WXLogin/{this.ControllerName}/SeniorAuthResponse/{AuthenticationScheme}");
        //    //return Content(url);
        //    return Redirect(url);
        //}
        //[Route("cissy/core/WXLogin/[controller]/SeniorAuthResponse/{AuthenticationScheme}")]
        //public async Task<IActionResult> SeniorAuthResponse(string AuthenticationScheme, string code, string state)
        //{
        //    if (WeiXinMpConfig.IsNull())
        //        return Redirect(WeiXinMpConfig.ErrUrl);
        //    string url = Actor.Public.GetCodeAccessTokenUrl(WeiXinMpConfig.AppId, WeiXinMpConfig.AppSecret, code);
        //    string redirectUrl = WeiXinMpConfig.ErrUrl;
        //    AccessTokenResult atr = default(AccessTokenResult);
        //    await Actor.Public.GetJsonAsync<AccessTokenResult>(url, result =>
        //    {
        //        atr = result;
        //    });
        //    if (atr.IsNotNull())
        //    {
        //        string infoUrl = Actor.Public.GeUserInfoUrl(atr.access_token, atr.openid);
        //        //return Content(infoUrl);
        //        await Actor.Public.GetJsonAsync<WXUserInfo>(infoUrl, userinfo =>
        //        {
        //            //把拉取用户的UUID和昵称传递给AccessTokenResult
        //            atr.unionid = userinfo.unionid;
        //            atr.nick = userinfo.nickname.UrlEncode();
        //            redirectUrl = Actor.Public.GetWeiXinSTSUrl(WeiXinMpConfig, atr, WeiXinMpConfig.AuthBaseUrl + $"/cissy/core/WXLogin/{this.ControllerName}/SignIn/{AuthenticationScheme}", WeiXinMpConfig.AuthBaseUrl + $"/cissy/core/WXLogin/{this.ControllerName}/GetSeniorAuth/{AuthenticationScheme}");
        //        });
        //    }
        //    return Redirect(redirectUrl);
        //}
    }
}
