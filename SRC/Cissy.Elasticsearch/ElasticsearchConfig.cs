using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Cissy.Configuration;

namespace Cissy.Elasticsearch
{
    /// <summary>
    /// Redis配置
    /// </summary>
    public class ElasticsearchConfig : IConfigModel
    {
        public string ConfigName { get { return "elasticsearch"; } }
        public string UserName;
        public string Password;
        public string Uri;
        public List<string> Connections = new List<string>();
        public void InitConfig(IConfigurationSection section)
        {
            UserName = section["username"];
            Password = section["password"];
            Uri = section["uri"];
        }
    }
}
