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
using Cissy.WeiXin;
using System.Security.Claims;
using Cissy.Configuration;
using Microsoft.AspNetCore.WebUtilities;

namespace Cissy.IS4
{
    public abstract class WeiXinWebAuthBaseController : BaseAuthController, IWeiXinAuthController
    {
        protected ICissyConfig CissyConfig;
        protected WeiXinMpConfig WeiXinMpConfig;
        protected string ControllerName { get { return this.ControllerContext.ActionDescriptor.ControllerName; } }
        public WeiXinWebAuthBaseController(ICissyConfig cissyConfig)
        {
            CissyConfig = cissyConfig;
            WeiXinMpConfig = CissyConfig.GetConfig<WeiXinMpConfig>();
        }
        public abstract void PreSignInAspect(WeiXinPassport passport, ClaimsPrincipal pricipal);

        [HttpGet]
        public abstract IActionResult DoSignIn(string AuthenticationScheme, ClaimsPrincipal pricipal);

        [HttpGet]
        [Route("cissy/core/WXQRLogin/[controller]/SignIn/{AuthenticationScheme}")]
        public IActionResult SignIn(string AuthenticationScheme, string token)
        {
            try
            {
                DefaultJwtPrincipalBuilder builder = new DefaultJwtPrincipalBuilder(WeiXinMpConfig.JwtSecret);
                var claimsPrincipal = builder.GetPrincipal(AuthenticationScheme, token);
                PreSignInAspect(claimsPrincipal.GetWeiXinPassport(), claimsPrincipal);
                return DoSignIn(AuthenticationScheme, claimsPrincipal);
            }
            catch (Exception ep)
            {
                return Redirect("~/");
                //return Content(AuthenticationScheme + "////" + ep.Message + "///" + ep.StackTrace);
            }
        }
        [HttpGet]
        [Route("cissy/core/WXQRLogin/[controller]/GetAuth/{AuthenticationScheme}")]
        public IActionResult GetAuth(string AuthenticationScheme)
        {
            if (WeiXinMpConfig.IsNull())
                return Redirect(WeiXinMpConfig.ErrUrl);
            string url = Actor.Public.GetWeiXinWebSTSUrl(WeiXinMpConfig, (this.Request.BaseUrl() + $"/cissy/core/WXQRLogin/{this.ControllerName}/SignIn/{AuthenticationScheme}"));
            return Redirect(url);
        }
    }
}
