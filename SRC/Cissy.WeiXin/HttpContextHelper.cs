using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Cissy.WeiXin
{
    public static class HttpContextHelper
    {
        //string webRoot = Request.Url.Scheme + "://" + Request.Url.Authority;
        public static string BaseUrl(this HttpRequest request)
        {
            //return request.Scheme + (request.IsHttps ? "s://" : "://") + request.Host.Host + (request.Host.Port.HasValue ? ":" + request.Host.Port : string.Empty);
            return request.Scheme + "://" + request.Host.Host + (request.Host.Port.HasValue ? ":" + request.Host.Port : string.Empty);
        }
        public static string BaseUrlSsl(this HttpRequest request)
        {
            //return request.Scheme + (request.IsHttps ? "s://" : "://") + request.Host.Host + (request.Host.Port.HasValue ? ":" + request.Host.Port : string.Empty);
            return "https://" + request.Host.Host + (request.Host.Port.HasValue ? ":" + request.Host.Port : string.Empty);
        }
    }
}
