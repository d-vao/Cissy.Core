using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Cissy.Configuration;
namespace Cissy.AOS
{
    /// <summary>
    /// Redis配置
    /// </summary>
    public class AliOSSConfig : IConfigModel
    {
        public string ConfigName { get { return "alioss"; } }
        public string Endpoint { get; set; }
        public string BucketDomain { get; set; }
        public string Bucket { get; set; }
        public string AccessKeyId { get; set; }
        public string AccessKeySecret { get; set; }
      

        public List<string> Connections = new List<string>();
        public void InitConfig(IConfigurationSection section)
        {
            Endpoint = section["endpoint"];
            BucketDomain = section["bucketdomain"];
            Bucket = section["bucket"];
            AccessKeyId = section["accesskeyid"];
            AccessKeySecret = section["accesskeysecret"];
           
        }
    }
}
