using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
namespace Cissy.IS4
{

    public class BaseAuthController : ApiController
    {
        public const string DefaultSchemeName = "cissy_cookie";
        public const string DefaultCookiePrefix = "cissyauth";
        [HttpGet]
        [Route("cissy/core/resource/js/swaggercn.js")]
        public IActionResult GetJs()
        {
            var stream = typeof(IWeiXinAuthController).Assembly.GetManifestResourceStream("Cissy.IS4.swagger_translator.js");
            return new FileStreamResult(stream, "application/x-javascript");
        }
    }
}
