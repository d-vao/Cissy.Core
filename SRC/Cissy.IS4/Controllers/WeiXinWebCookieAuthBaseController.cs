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

namespace Cissy.IS4
{
    public abstract class WeiXinWebCookieAuthBaseController : WeiXinWebAuthBaseController
    {
        public WeiXinWebCookieAuthBaseController(ICissyConfig cissyConfig) : base(cissyConfig)
        { }
        [HttpGet]
        public override IActionResult DoSignIn(string AuthenticationScheme, ClaimsPrincipal pricipal)
        {
            HttpContext.SignInAsync(AuthenticationScheme, pricipal, new AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddDays(20)
            });
            //DefaultJwtPrincipalBuilder builder = new DefaultJwtPrincipalBuilder(this.WeiXinMpConfig.JwtSecret);
            //HttpContext.Response.Cookies.Append(DefaultSchemeName, builder.BuildToken(pricipal.Claims), new Microsoft.AspNetCore.Http.CookieOptions()
            //{
            //    HttpOnly = true
            //});
            return Redirect(this.WeiXinMpConfig.CallBackUrl);
        }
    }
}
