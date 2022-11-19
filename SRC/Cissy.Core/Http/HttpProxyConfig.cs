using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Cissy.Configuration;

namespace Cissy.Http
{
    /// <summary>
    /// Http代理配置
    /// </summary>
    public class HttpProxyConfig : IConfigModel
    {
        public string ConfigName { get { return "httpproxy"; } }
        public string Uri;
        public string UserName;
        public string Password;
        public void InitConfig(IConfigurationSection section)
        {
            Uri = section["uri"];
            UserName = section["username"];
            Password = section["password"];
        }
    }
}
