using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
namespace Cissy.Configuration
{
    /// <summary>
    /// 微信公众号配置
    /// </summary>
    public class WeiXinMpConfig : IConfigModel
    {
        public string ConfigName { get { return "weixinmp"; } }
        public string AppId;
        public string AppSecret;
        public string AuthUrl;
        public string JwtSecret;
        public string CallBackUrl;
        public string ErrUrl;
        public void InitConfig(IConfigurationSection section)
        {
            AppId = section["appid"];
            AppSecret = section["appsecret"];
            AuthUrl = section["authurl"];
            JwtSecret = section["jwtsecret"];
            CallBackUrl = section["callbackurl"];
            ErrUrl = section["errurl"];
        }
    }
}
