using System;
using System.Collections.Generic;
using System.Text;
using Elasticsearch.Net;
using Nest;

namespace Cissy.Elasticsearch
{
    public interface IElasticsearchFactory
    {
        ElasticsearchConfig Config { get; }
        ElasticClient Build(string IndexName);
    }
}
