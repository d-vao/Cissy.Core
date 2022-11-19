using System;
using System.Collections.Generic;
using System.Text;

namespace Cissy.Authentication
{
    public class CissyAuthenticationOption
    {
        public string Scheme { get; set; }
        public AuthenticationTypes AuthenticationType { get; set; }
        public AuthenticationApplies AuthenticationApply { get; set; }
    }
    public static class AuthenticationScheme
    {
        public const string Default = "cyauth";
    }

    public enum AuthenticationTypes
    {
        Token = 1,
        Cookie = 2
    }
    public enum AuthenticationApplies
    {
        /// <summary>
        /// Web
        /// </summary>
        Web = 1,
        /// <summary>
        /// 微信二维码
        /// </summary>
        WxQR = 2,
        /// <summary>
        /// 微信公众号
        /// </summary>
        WxMp = 3,
        /// <summary>
        /// 微信小程序
        /// </summary>
        WxApp = 4
    }
}
