using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Cissy.Extensions;
using Cissy.Authentication;
using Cissy.Authentication.JWT;
using Cissy.Http;
using Cissy.Serialization;
using System.Security.Claims;
using Cissy.Configuration;
using Microsoft.AspNetCore.WebUtilities;
using Cissy.WeiXin;

namespace Cissy.Authentication
{
    public abstract class WeiXinAuthBaseController : ApiController
    {
        protected ICissyConfig CissyConfig;
        protected IWeiXinMpApi WeiXinMpApi;
        protected AppConfig appConfig;
        protected CissyAuthenticationOption Option;
        protected string ControllerName { get { return this.ControllerContext.ActionDescriptor.ControllerName; } }
        public WeiXinAuthBaseController(ICissyConfig cissyConfig)
        {
            CissyConfig = cissyConfig;
            appConfig = CissyConfig.GetConfig<AppConfig>();
            Option = Actor.Public.GetService<IAuthenticationOptionBuilder>().Build();
            WeiXinMpApi = Actor.Public.GetService<IWeiXinMpApi>();
        }
        public abstract void PreSignInAspect(CissyPassport passport, ClaimsPrincipal pricipal);
        public abstract IActionResult DoGrantIn(string group, string sessioncode, CissyPassport passport, ClaimsPrincipal pricipal);
        [HttpGet]
        public IActionResult DoSignIn(string AuthenticationScheme, ClaimsPrincipal pricipal)
        {
            if (this.Option.AuthenticationType == AuthenticationTypes.Cookie)
            {
                DefaultJwtPrincipalBuilder builder = new DefaultJwtPrincipalBuilder(this.appConfig.AuthSecret);
                //HttpContext.Response.Cookies.Append(AuthenticationHelper.BuildCookieName(this.Option.Scheme), builder.BuildToken(pricipal.Claims), new Microsoft.AspNetCore.Http.CookieOptions()
                //{
                //    Expires = DateTime.Now.AddDays(1),
                //    //HttpOnly = true
                //});
                HttpContext.Response.Cookies.Append(AuthenticationHelper.BuildCookieName(this.Option.Scheme), builder.BuildToken(pricipal.Claims));
                //return Content(builder.BuildToken(pricipal.Claims));
                return Redirect(this.appConfig.AuthBackUrl);
            }
            else if (this.Option.AuthenticationType == AuthenticationTypes.Token)
            {
                DefaultJwtPrincipalBuilder builder = new DefaultJwtPrincipalBuilder(this.appConfig.AuthSecret);
                //return Content(builder.BuildToken(pricipal.Claims));
                return Redirect($"{this.appConfig.AuthBackUrl}?token={builder.BuildToken(pricipal.Claims)}");
            }
            return StatusCode(500);
        }

        [HttpGet]
        [Route("cissy/core/auth/SignOut")]
        public IActionResult SignOut()
        {
            HttpContext.Response.Cookies.Delete(AuthenticationHelper.BuildCookieName(this.Option.Scheme));
            return Redirect("~/");
        }
        [HttpGet]
        [Route("cissy/core/auth/{AuthenticationScheme}")]
        public IActionResult SignIn(string AuthenticationScheme, string token)
        {
            try
            {
                DefaultJwtPrincipalBuilder builder = new DefaultJwtPrincipalBuilder(appConfig.AuthSecret);
                var claimsPrincipal = builder.GetPrincipal(AuthenticationScheme, token);
                PreSignInAspect(claimsPrincipal.GetWeiXinPassport(), claimsPrincipal);
                return DoSignIn(AuthenticationScheme, claimsPrincipal);
            }
            catch (Exception ep)
            {
                //return Redirect("~/");
                return Content(AuthenticationScheme + "////" + ep.Message + "///" + ep.StackTrace);
            }
        }
        [HttpGet]
        //[Route("cissy/core/[controller]/Auth/{AuthenticationScheme}")]
        public IActionResult Auth()
        {
            if (appConfig.IsNull())
                return Redirect(appConfig.ErrUrl);
            string url = default;
            if (this.Option.AuthenticationApply == AuthenticationApplies.Web)
            {
                url = Actor.Public.GetWxWebAuthUrl(appConfig, (this.Request.BaseUrl() + $"/cissy/core/auth/{this.Option.Scheme}"));
            }
            else if (this.Option.AuthenticationApply == AuthenticationApplies.WxQR)
            {
                url = Actor.Public.GetWxQRAuthUrl(appConfig, (this.Request.BaseUrl() + $"/cissy/core/auth/{this.Option.Scheme}"));
            }
            else if (this.Option.AuthenticationApply == AuthenticationApplies.WxMp)
            {
                url = Actor.Public.GetWxMpAuthUrl(appConfig, (this.Request.BaseUrl() + $"/cissy/core/auth/{this.Option.Scheme}"));
            }
            else
                return Content("微信小程序不支持跳转认证");
            //return Content(url);
            return Redirect(url);
        }

        [HttpGet]
        [Route("cissy/core/grant/{AuthenticationScheme}/{group}/{grantcode}")]
        public IActionResult GrantIn(string AuthenticationScheme, string token, string group, string grantcode)
        {
            try
            {
                DefaultJwtPrincipalBuilder builder = new DefaultJwtPrincipalBuilder(appConfig.AuthSecret);
                var claimsPrincipal = builder.GetPrincipal(AuthenticationScheme, token);
                return DoGrantIn(group, grantcode, claimsPrincipal.GetWeiXinPassport(), claimsPrincipal);
            }
            catch (Exception ep)
            {
                return Content($"{AuthenticationScheme}--{group}--{grantcode}--{token}");
                //return Redirect("~/");
                //return Content(AuthenticationScheme + "////" + ep.Message + "///" + ep.StackTrace);
            }
        }
        [HttpGet]
        //[Route("cissy/core/[controller]/Grant/{AuthenticationScheme}")]
        public IActionResult Grant(string group, string grantcode, string message)
        {
            try
            {
                if (appConfig.IsNull())
                    return Redirect(appConfig.ErrUrl);
                string url = default;
                if (this.Option.AuthenticationApply == AuthenticationApplies.Web)
                {
                    url = Actor.Public.GetWxWebAuthUrl(appConfig, (this.Request.BaseUrl() + $"/cissy/core/grant/{this.Option.Scheme}/{group}/{grantcode}"), 1, message);
                }
                else if (this.Option.AuthenticationApply == AuthenticationApplies.WxQR)
                {
                    url = Actor.Public.GetWxQRAuthUrl(appConfig, (this.Request.BaseUrl() + $"/cissy/core/grant/{this.Option.Scheme}/{group}/{grantcode}"), 1, message);
                }
                else if (this.Option.AuthenticationApply == AuthenticationApplies.WxMp)
                {
                    url = Actor.Public.GetWxMpAuthUrl(appConfig, (this.Request.BaseUrl() + $"/cissy/core/grant/{this.Option.Scheme}/{group}/{grantcode}"), 1, message);
                }
                else
                    return Content("微信小程序不支持跳转认证");
                //return Content(url);
                return Redirect(url);
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetAppAuthToken(string openId, string unionId, string nickName, string avatarUrl)
        {
            var wxaToken = buildWXAToken(this.appConfig, openId, unionId, nickName, avatarUrl);
            var url = Actor.Public.GetWxAppAuthUrl(this.appConfig, wxaToken);
            string token = string.Empty;
            if (await Actor.Public.GetAsync(url, m =>
            {
                token = m;
            }))
            {
                DefaultJwtPrincipalBuilder builder = new DefaultJwtPrincipalBuilder(this.appConfig.AuthSecret);
                var claimsPrincipal = builder.GetPrincipal(this.Option.Scheme, token);
                PreSignInAspect(claimsPrincipal.GetWeiXinPassport(), claimsPrincipal);
                DefaultJwtPrincipalBuilder b2 = new DefaultJwtPrincipalBuilder(this.appConfig.AuthSecret);
                return Content(b2.BuildToken(claimsPrincipal.Claims));
            }
            return StatusCode(500);
        }
        [HttpPost]
        public async Task<IActionResult> GetAuthToken(string code, string encryptedData, string iv)
        {
            if (appConfig.IsNull())
                return StatusCode(500);
            var result = await WeiXinMpApi.Code2SessionAsync(code);
            if (result.IsNull())
                return StatusCode(500);
            var session_key = result.session_key;
            var json = WXEncryptHelper.DecodeEncryptedData(session_key, encryptedData, iv);
            var userInfo = json.JsonToModel<DecodedUserInfo>();
            if (userInfo.unionId.IsNullOrEmpty())
                return StatusCode(406);
            var wxaToken = buildWXAToken(this.appConfig, userInfo.openId, userInfo.unionId, userInfo.nickName, userInfo.avatarUrl);
            var url = Actor.Public.GetWxAppAuthUrl(this.appConfig, wxaToken);
            string token = string.Empty;
            if (await Actor.Public.GetAsync(url, m =>
            {
                token = m;
            }))
            {
                DefaultJwtPrincipalBuilder builder = new DefaultJwtPrincipalBuilder(this.appConfig.AuthSecret);
                var claimsPrincipal = builder.GetPrincipal(this.Option.Scheme, token);
                PreSignInAspect(claimsPrincipal.GetWeiXinPassport(), claimsPrincipal);
                DefaultJwtPrincipalBuilder b2 = new DefaultJwtPrincipalBuilder(this.appConfig.AuthSecret);
                return Content(b2.BuildToken(claimsPrincipal.Claims));
            }
            return StatusCode(500);
        }
        string buildWXAToken(AppConfig config, string OpenId, string UnionId, string NickName, string Avatar)
        {
            DefaultJwtPrincipalBuilder builder = new DefaultJwtPrincipalBuilder(config.AuthSecret);
            List<Claim> list = new List<Claim>();
            list.Add(new Claim(CissyWXATokenClaims.OpenId, OpenId));
            list.Add(new Claim(CissyWXATokenClaims.UnionId, UnionId));
            list.Add(new Claim(CissyWXATokenClaims.NickName, NickName));
            list.Add(new Claim(CissyWXATokenClaims.Avatar, Avatar));
            return builder.BuildToken(list);
        }
    }
}
