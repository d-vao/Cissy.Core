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
using Cissy.WeiXin;
using Cissy.Authentication;
using Cissy.Serialization;
using System.Security.Claims;
using Cissy.Configuration;
using Microsoft.AspNetCore.WebUtilities;


namespace Cissy.IS4
{
    public abstract class WeiXinMpJwtAuthBaseController : WeiXinMpAuthBaseController
    {
        public WeiXinMpJwtAuthBaseController(ICissyConfig cissyConfig) : base(cissyConfig)
        { }
        [HttpGet]
        public override IActionResult DoSignIn(string AuthenticationScheme, ClaimsPrincipal pricipal)
        {
            DefaultJwtPrincipalBuilder builder = new DefaultJwtPrincipalBuilder(this.WeiXinMpConfig.JwtSecret);

            //return Content(builder.BuildToken(pricipal.Claims));
            return Redirect($"{this.WeiXinMpConfig.CallBackUrl}?token={builder.BuildToken(pricipal.Claims)}");
        }
        [HttpGet]
        public IActionResult Auth()
        {
            return Redirect(Actor.Public.GetMpLoginPath(this.ControllerName, DefaultSchemeName));
        }
    }
}
