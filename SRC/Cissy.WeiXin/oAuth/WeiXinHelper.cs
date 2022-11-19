using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Cissy.Extensions;
namespace Cissy.WeiXin.oAuth
{
    /// <summary>
    /// 获取OAuth AccessToken的结果
    /// 如果错误，返回结果{"errcode":40029,"errmsg":"invalid code"}
    /// </summary>
    [Serializable]
    public class AccessTokenResult : IModel
    {
        /// <summary>
        /// 接口调用凭证
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// access_token接口调用凭证超时时间，单位（秒）
        /// </summary>
        public int expires_in { get; set; }
        /// <summary>
        /// 用户刷新access_token
        /// </summary>
        public string refresh_token { get; set; }
        /// <summary>
        /// 授权用户唯一标识
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 用户授权的作用域，使用逗号（,）分隔
        /// </summary>
        public string scope { get; set; }
        /// <summary>
        /// 开放平台用户ID
        /// </summary>
        public string unionid;
        /// <summary>
        /// 昵称
        /// </summary>
        public string nick;
        /// <summary>
        /// 头像地址
        /// </summary>
        public string avatar;
    }
    /// <summary>
    /// 微信用户信息
    /// </summary>
    [Serializable]
    public class WXUserInfo : IModel
    {
        public string openid;
        public string nickname;
        public string sex;
        public string province;
        public string city;
        public string country;
        public string headimgurl;
        public string[] privilege;
        public string unionid;
    }
    public static class WeiXinHelper
    {
        public static string GetAuthorizeUrl(this HttpRequest request, string AppId, string ReturnPath)
        {
            return "https://open.weixin.qq.com/connect/oauth2/authorize?" + $"appid={AppId}&redirect_uri={(request.BaseUrl() + ReturnPath).AsUrlData()}&response_type=code&scope=snsapi_base&state={AppId}#wechat_redirect";
        }

        public static string GetSeniorAuthorizeUrl(this HttpRequest request, string AppId, string ReturnPath)
        {
            return "https://open.weixin.qq.com/connect/oauth2/authorize?" + $"appid={AppId}&redirect_uri={(request.BaseUrl() + ReturnPath).AsUrlData()}&response_type=code&scope=snsapi_userinfo&state={AppId}#wechat_redirect";
        }
        public static string GetCodeAccessTokenUrl(this Public Public, string AppId, string AppSecret, string Code)
        {
            return "https://api.weixin.qq.com/sns/oauth2/access_token?" + $"appid={AppId}&secret={AppSecret}&code={Code}&grant_type=authorization_code";
        }

        public static string GeUserInfoUrl(this Public Public, string AccessToken, string OpenId)
        {
            return "https://api.weixin.qq.com/sns/userinfo?" + $"access_token={AccessToken}&openid={OpenId}&lang=zh_CN";
        }

    }
}
