﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Cissy.Extensions;
using Cissy.IS4;
using Cissy.WeiXin;
using Cissy.IS4.JWT;
using Cissy.Http;
using Cissy.Authentication;
using Cissy.Serialization;
using System.Security.Claims;
using Cissy.Configuration;
using Microsoft.AspNetCore.WebUtilities;

namespace Cissy.IS4
{
    public abstract class WeiXinWebJwtAuthBaseController : WeiXinWebAuthBaseController
    {
        public WeiXinWebJwtAuthBaseController(ICissyConfig cissyConfig) : base(cissyConfig)
        { }
        [HttpGet]
        public override IActionResult DoSignIn(string AuthenticationScheme, ClaimsPrincipal pricipal)
        {
            DefaultJwtPrincipalBuilder builder = new DefaultJwtPrincipalBuilder(this.WeiXinMpConfig.JwtSecret);

            //return Content(builder.BuildToken(pricipal.Claims));
            //var json = new JwtBuilder()
            //    .WithSecret(_WeiXinMpConfig.JwtSecret)
            //    .MustVerifySignature()
            //.Decode(builder.BuildToken(pricipal.Claims));
            //return Content(json);
            //var t = from c in pricipal.Claims select new { c.Type, c.Value };
            //var s = Newtonsoft.Json.JsonConvert.SerializeObject(t);
            //return Content(s);
            var url = $"{this.WeiXinMpConfig.CallBackUrl}?token={builder.BuildToken(pricipal.Claims)}";
            //return Content(url);
            return Redirect(url);
        }
        [HttpGet]
        public IActionResult Auth()
        {
            return Redirect(Actor.Public.GetWebLoginPath(this.ControllerName, DefaultSchemeName));
        }
    }
}
