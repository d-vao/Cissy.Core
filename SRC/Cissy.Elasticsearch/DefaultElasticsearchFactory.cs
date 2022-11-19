using System;
using System.Collections.Generic;
using System.Text;
using Nest;

namespace Cissy.Elasticsearch
{
    public class DefaultElasticsearchFactory : IElasticsearchFactory
    {
        public ElasticsearchConfig Config { get; internal set; }
        public ElasticClient Build(string IndexName)
        {
            var uri = new Uri(Config.Uri);
            var c = new ConnectionSettings(uri);
            c.BasicAuthentication(Config.UserName, Config.Password);
            var client = new ElasticClient(c.DefaultIndex(IndexName));
            //var rp = client.UpdateIndexSettings(new UpdateIndexSettingsRequest("cissy")
            //{
            //    //IndexSettings indexSettings=new IndexSettings (){ NumberOfReplicas=1, RefreshInterval=-1};
            //    IndexSettings = new IndexSettings()
            //    {
            //        NumberOfReplicas = 10, //副本数量
            //        RefreshInterval = 3, //分片Refresh时间间隔
            //        NumberOfShards = 100,
            //        NumberOfRoutingShards = 2
            //    }
            //});
            return client;
        }
    }
}
