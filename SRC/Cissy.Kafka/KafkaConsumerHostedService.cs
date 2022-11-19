using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace Cissy.Kafka
{
    public abstract class KafkaConsumerHostedService<T> : BackgroundService where T : IIntegrationEvent
    {
        protected readonly IServiceProvider _services;
        protected readonly IConfiguration _config;
        protected readonly ILogger<KafkaConsumerHostedService<T>> _logger;

        public KafkaConsumerHostedService(IServiceProvider services, IConfiguration config, ILogger<KafkaConsumerHostedService<T>> logger)
        {
            _services = services;
            _config = config;
            _logger = logger;
        }

        /// <summary>
        /// 消费该事件，比如调用 Application Service 持久化数据等
        /// </summary>
        /// <param name="event">事件内容</param>
        protected abstract void DoWork(T @event);

        /// <summary>
        /// 构造 Kafka 消费者实例，监听指定 Topic，获得最新的事件
        /// </summary>
        /// <param name="stoppingToken">终止标识</param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Factory.StartNew(() =>
            {
                var topic = _config.GetValue<string>($"Kafka:Topics:{typeof(T).Name}");

                var consumerConfig = new ConsumerConfig
                {
                    BootstrapServers = _config.GetValue<string>("Kafka:BootstrapServers"),
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    GroupId = _config.GetValue<string>("Application:Name"),
                    EnableAutoCommit = true,
                };
                var builder = new ConsumerBuilder<string, string>(consumerConfig);
                using (var consumer = builder.Build())
                {
                    consumer.Subscribe(topic);
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        try
                        {
                            var result = consumer.Consume(stoppingToken);
                            var @event = JsonConvert.DeserializeObject<T>(result.Value);
                            DoWork(@event);
                            //consumer.StoreOffset(result);
                        }
                        catch (OperationCanceledException ex)
                        {
                            consumer.Close();
                            _logger.LogDebug(ex, "Kafka 消费者结束，退出后台线程");
                        }
                        catch (AbpValidationException ex)
                        {
                            _logger.LogError(ex, $"Kafka {GetValidationErrorNarrative(ex)}");
                        }
                        catch (ConsumeException ex)
                        {
                            _logger.LogError(ex, "Kafka 消费者产生异常");
                        }
                        catch (KafkaException ex)
                        {
                            _logger.LogError(ex, "Kafka 产生异常");
                        }
                        catch (ValidationException ex)
                        {
                            _logger.LogError(ex, "Kafka 消息验证失败");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Kafka 捕获意外异常");
                        }
                    }
                }
            }, stoppingToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        private string GetValidationErrorNarrative(AbpValidationException validationException)
        {
            var detailBuilder = new StringBuilder();
            detailBuilder.AppendLine("验证过程中检测到以下错误");

            foreach (var validationResult in validationException.ValidationErrors)
            {
                detailBuilder.AppendFormat(" - {0}", validationResult.ErrorMessage);
                detailBuilder.AppendLine();
            }

            return detailBuilder.ToString();
        }
    }

}
