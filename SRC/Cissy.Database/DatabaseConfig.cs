using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Cissy.Configuration;
namespace Cissy.Database
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    public class DatabaseConfig : IConfigModel
    {
        public string ConfigName { get { return "database"; } }
        public List<DbConnectionConfig> Connections = new List<DbConnectionConfig>();
        public void InitConfig(IConfigurationSection section)
        {
            foreach (IConfigurationSection dbsection in section.GetChildren())
            {
                DbConnectionConfig connectionConfig = new DbConnectionConfig();
                connectionConfig.InitConfig(dbsection);
                Connections.Add(connectionConfig);
            }
        }
    }
    public class DbConnectionConfig : IConfigModel
    {
        public string ConfigName { get { return "connection"; } }
        public string Name;
        public string ConnectionString;
        public void InitConfig(IConfigurationSection section)
        {
            Name = section["name"];
            ConnectionString = section["connectionstring"];
        }
        public string GetConnectionString()
        {
            return ConnectionString;
        }
    }
}
