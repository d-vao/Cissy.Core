using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
namespace Cissy.Configuration
{
    /// <summary>
    /// 应用配置
    /// </summary>
    public class AppConfig : IConfigModel
    {
        public string ConfigName { get { return "app"; } }
        public string AppId;
        public string AppSecret;
        public string AuthUrl;
        public string AuthSecret;
        public string AuthBackUrl;
        public string ErrUrl;
        public void InitConfig(IConfigurationSection section)
        {
            AppId = section["appid"];
            AppSecret = section["appsecret"];
            AuthUrl = section["authurl"];
            AuthSecret = section["authsecret"];
            AuthBackUrl = section["authbackurl"];
            ErrUrl = section["errurl"];
        }
    }
}
