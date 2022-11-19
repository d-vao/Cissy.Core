using System;
using System.Collections.Generic;
using System.Text;
using Confluent.Kafka;

namespace Cissy.Kafka
{
    public interface IKafakaFactory
    {
        KafkaConfig Config { get; }
        IAdminClient BuildAdminClient();
        IProducer<TKey, TValue> BuildProducer<TKey, TValue>();
        IConsumer<TKey, TValue> BuildConsumer<TKey, TValue>(string ConsumerGroupId);
    }
}
