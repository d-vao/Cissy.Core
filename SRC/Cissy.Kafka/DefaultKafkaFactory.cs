using System;
using System.Collections.Generic;
using System.Text;
using Confluent.Kafka;


namespace Cissy.Kafka
{
   public sealed class DefaultKafkaFactory : IKafakaFactory
    {
        public KafkaConfig Config { get; set; }
        internal DefaultKafkaFactory()
        { }
        public IAdminClient BuildAdminClient()
        {
            return new AdminClientBuilder(new AdminClientConfig { BootstrapServers = Config.Servers }).Build();
        }
        public IProducer<TKey, TValue> BuildProducer<TKey, TValue>()
        {
            var config = new ProducerConfig { BootstrapServers = Config.Servers };
            return new ProducerBuilder<TKey, TValue>(config).Build();
        }
        public IConsumer<TKey, TValue> BuildConsumer<TKey, TValue>(string ConsumerGroupId)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = Config.Servers,
                GroupId = ConsumerGroupId,
                EnableAutoCommit = true
            };
            return new ConsumerBuilder<TKey, TValue>(config).Build();
        }
    }
}
