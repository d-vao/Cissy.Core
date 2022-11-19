using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Cissy.Configuration;

namespace Cissy.Kafka
{
    /// <summary>
    /// Kafka连接配置
    /// </summary>
    public class KafkaConfig : IConfigModel
    {
        public string ConfigName { get { return "kafka"; } }
        public string Servers;
        public string ConsumerGroupId;
        public string Topic;
        public string PartitionNumber;
        public void InitConfig(IConfigurationSection section)
        {
            Servers = section["servers"];
            ConsumerGroupId = section["consumergroupid"];
            Topic = section["topic"];
            PartitionNumber = section["partitionnumber"];
        }
    }
}
