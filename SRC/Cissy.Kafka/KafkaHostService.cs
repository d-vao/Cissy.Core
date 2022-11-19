using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace Cissy.Kafka
{
    public abstract class KafkaHostService : BackgroundService
    {
        IKafakaFactory _KafakaFactory;
        KafkaConfig _config;
        public void InitService(IKafakaFactory KafakaFactory, KafkaConfig config)
        {
            _KafakaFactory = KafakaFactory;
            _config = config;
        }
        /// <summary>
        /// 消费该事件，比如调用 Application Service 持久化数据等
        /// </summary>
        /// <param name="event">事件内容</param>
        protected abstract void DoWork(ConsumeResult<Null, byte[]> consumeResult);
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Factory.StartNew(() =>
            {
                var config = new ConsumerConfig
                {
                    GroupId = _config.ConsumerGroupId,
                    BootstrapServers = _config.Servers,
                    EnableAutoCommit = true
                };

                using (var consumer = this._KafakaFactory.BuildConsumer<Null, byte[]>(this._KafakaFactory.Config.ConsumerGroupId))
                {
                    if (int.TryParse(_config.PartitionNumber, out int Num))
                    {
                        List<TopicPartitionOffset> list = new List<TopicPartitionOffset>();
                        for (int i = 0; i < Num; i++)
                        {
                            list.Add(new TopicPartitionOffset(new TopicPartition(_config.Topic, i), Offset.End));
                        }
                        consumer.Assign(_config.Topic.Select(topic => new TopicPartitionOffset(_config.Topic, 0, Offset.Beginning)).ToList());
                        try
                        {
                            while (!stoppingToken.IsCancellationRequested)
                            {
                                try
                                {
                                    var consumeResult = consumer.Consume();
                                    DoWork(consumeResult);
                                    Console.WriteLine($"Received message at {consumeResult.TopicPartitionOffset}: ${consumeResult.Value}");
                                }
                                catch (ConsumeException e)
                                {
                                    Console.WriteLine($"Consume error: {e.Error.Reason}");
                                }
                            }
                        }
                        catch (OperationCanceledException)
                        {
                            Console.WriteLine("Closing consumer.");
                            consumer.Close();
                        }
                    }
                }
            });
        }
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
