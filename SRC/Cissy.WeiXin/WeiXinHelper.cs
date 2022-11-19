using System;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using Cissy.Extensions;
using Cissy.Configuration;

namespace Cissy.WeiXin
{

    public static class WeiXinHelper
    {
        public static string GetWXALoginPath(this Public Public, string ControllerName, string AuthenticationScheme)
        {
            return $"/cissy/core/WXALogin/{ControllerName}/GetAuth/{AuthenticationScheme}";
        }
        public static string GetMpLoginPath(this Public Public, string ControllerName, string AuthenticationScheme)
        {
            return $"/cissy/core/WXLogin/{ControllerName}/GetAuth/{AuthenticationScheme}";
        }
        public static string GetWebLoginPath(this Public Public, string ControllerName, string AuthenticationScheme)
        {
            return $"/cissy/core/WXQRLogin/{ControllerName}/GetAuth/{AuthenticationScheme}";
        }
        public static string GetMpAuthorizeUrl(this Public Public, AppConfig config, string ReturnUrl)
        {
            return $"{config.AuthUrl}/account/WXMpLogin?AppId={config.AppId}&ReturnUrl={ReturnUrl.AsUrlData()}";
        }
        public static string GetWXAAuthorizeUrl(this Public Public, AppConfig config, string OpenId, string UnionId, string NickName, string Avatar)
        {
            return $"{config.AuthUrl}/account/WXALogin?AppId={config.AppId}&OpenId={OpenId}&UnionId={UnionId}&NickName={NickName}&Avatar={Avatar}";
        }
        public static string GetWeiXinWebSTSUrl(this Public Public, AppConfig config, string ReturnUrl)
        {
            return $"{config.AuthUrl}/account/wxqrlogin?AppId={config.AppId}&ReturnUrl={ReturnUrl.AsUrlData()}";
        }



        public static string GetWXAuthUrl(this Public Public, AppConfig config, string ReturnUrl)
        {
            return $"{config.AuthUrl}/account/wxqrlogin?AppId={config.AppId}&ReturnUrl={ReturnUrl.AsUrlData()}";
        }
    }
}
