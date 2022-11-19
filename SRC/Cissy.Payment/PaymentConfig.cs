using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Cissy.Configuration;
namespace Cissy.Payment
{
    /// <summary>
    /// 支付配置
    /// </summary>
    public class PaymentConfig : IConfigModel
    {
        public string ConfigName { get { return "payment"; } }
        public string AppName;
        public string AppSecret;
        public string PayUrl;
        public void InitConfig(IConfigurationSection section)
        {
            AppName = section["appname"];
            AppSecret = section["appsecret"];
            PayUrl = section["payurl"];
        }
    }
}
