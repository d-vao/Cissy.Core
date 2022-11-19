using System;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using Cissy.Extensions;
using Cissy.Configuration;

namespace Cissy.Authentication
{
    public static class CissyWXATokenClaims
    {
        public const string OpenId = "_cissy_wxatoken_openid";
        public const string UnionId = "_cissy_wxatoken_unionid";
        public const string NickName = "_cissy_wxatoken_nickname";
        public const string Avatar = "_cissy_wxatoken_avatar";
    }
    public static class AuthCenterHelper
    {
        public static string GetWxMpAuthUrl(this Public Public, AppConfig config, string ReturnUrl, int grant = 0, string message = "")
        {
            return $"{config.AuthUrl}/account/WXMpLogin?AppId={config.AppId}&ReturnUrl={ReturnUrl.AsUrlData()}&grant={grant}&message={message.UrlEncode()}";
        }
        //public static string GetWxAppAuthUrl(this Public Public, AppConfig config, string OpenId, string UnionId, string NickName, string Avatar)
        //{
        //    return $"{config.AuthUrl}/account/WXALogin?AppId={config.AppId}&OpenId={OpenId}&UnionId={UnionId}&NickName={NickName}&Avatar={Avatar}";
        //}
        public static string GetWxAppAuthUrl(this Public Public, AppConfig config, string token)
        {
            return $"{config.AuthUrl}/account/WXAAuth?AppId={config.AppId}&token={token}";
        }
        public static string GetWxWebAuthUrl(this Public Public, AppConfig config, string ReturnUrl, int grant = 0, string message = "")
        {
            return $"{config.AuthUrl}/account/WebLogin?AppId={config.AppId}&ReturnUrl={ReturnUrl.AsUrlData()}&grant={grant}&message={message.UrlEncode()}";
        }
        public static string GetWxQRAuthUrl(this Public Public, AppConfig config, string ReturnUrl, int grant = 0, string message = "")
        {
            return $"{config.AuthUrl}/account/wxqrlogin?AppId={config.AppId}&ReturnUrl={ReturnUrl.AsUrlData()}&grant={grant}&message={message.UrlEncode()}";
        }
        public static string GetDisplaceUserUrl(this Public Public, AppConfig config, string openId, string unionId)
        {
            return $"{config.AuthUrl}/user/displaceuser?AppId={config.AppId}&openId={openId}&unionId={unionId}";

        }
    }
}
