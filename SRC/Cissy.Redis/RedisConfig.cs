using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Cissy.Configuration;
namespace Cissy.Redis
{
    /// <summary>
    /// Redis配置
    /// </summary>
    public class RedisConfig : IConfigModel
    {
        public string ConfigName { get { return "redis"; } }
        public string Name;
        public string Db;
        public List<string> Connections = new List<string>();
        public void InitConfig(IConfigurationSection section)
        {
            Name = section["name"];
            Db = section["db"];
            IConfigurationSection connectionsSection = section.GetSection("connections");
            if (connectionsSection.IsNotNull())
            {
                Connections.AddRange(connectionsSection.GetChildren().Select(p => p.Value));
            }
        }
    }
}
