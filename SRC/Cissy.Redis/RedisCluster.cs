using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Linq;
using ServiceStack.Redis;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Cissy;
using Cissy.Caching;
using Cissy.Caching.Redis;

namespace Cissy.Redis
{
    public delegate void MessageHandler(string msg);
    public abstract partial class RedisCluster : IRedisCache
    {
        public static int PoolSize = 100;
        public static int PoolTimeOutSeconds = 10;
        public string Name { get; private set; }
        public string[] RedisConnections { get; private set; }
        public long? Db;
        ConcurrentDictionary<string, MessageHandler> MessageTypes;
        PooledRedisClientManager redisPoolManager;
        public bool Started { get; private set; }
        public RedisCluster()
        { }
        public void Init(string Name, string[] Connections)
        {
            this.Name = Name;
            this.RedisConnections = Connections;
            ServiceStack.Redis.RedisConfig.VerifyMasterConnections = false;//需要设置
            redisPoolManager = new PooledRedisClientManager(PoolSize/*连接池个数*/, PoolTimeOutSeconds/*连接池超时时间*/, RedisConnections);
        }
        public IRedisClient GetRedisClient()
        {
            IRedisClient redisClient = redisPoolManager.GetClient();//获取连接
            if (this.Db.HasValue)
                redisClient.Db = this.Db.Value;
            RedisNativeClient redisNativeClient = (RedisNativeClient)redisClient;
            redisNativeClient.Client = null;//ApsaraDB for Redis不支持client setname所以这里需要显示的把client对象置为null
            return redisClient;
        }
        public virtual void Start()
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                IRedisSubscription subscription = redisClient.CreateSubscription();
                MessageTypes = new ConcurrentDictionary<string, MessageHandler>();
                RegisterMessage(MessageTypes);
                //接受到消息时
                subscription.OnMessage = HandMessage;
                //订阅频道时
                subscription.OnSubscribe = OnRegisterMessage;
                //取消订阅频道时
                subscription.OnUnSubscribe = OnUnRegisterMessage;
                if (MessageTypes.IsNotNullAndEmpty())
                {
                    subscription.SubscribeToChannels(MessageTypes.Keys.ToArray());
                }
                Started = true;
            }
        }
        public void PublishMessage<T>(string MessageType, T Message) where T : IModel
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                redisClient.PublishMessage(MessageType, Message.ModelToJson());
            }
        }
        /// <summary>
        /// 注册消息
        /// </summary>
        public abstract void RegisterMessage(ConcurrentDictionary<string, MessageHandler> MessageTypes);
        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="MessageType"></param>
        /// <param name="Msg"></param>
        void HandMessage(string MessageType, string Msg)
        {
            MessageHandler handler;
            if (MessageTypes.TryGetValue(MessageType, out handler))
            {
                handler(Msg);
            }
        }
        /// <summary>
        /// 注册消息事件
        /// </summary>
        /// <param name="MessageType"></param>
        public abstract void OnRegisterMessage(string MessageType);
        /// <summary>
        /// 注销消息事件 
        /// </summary>
        /// <param name="MessageType"></param>
        public abstract void OnUnRegisterMessage(string MessageType);
        #region ICach
        public async Task<bool> ContainsKeyAsync<T>(CacheKey key) where T : class, IModel
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                return await Task.FromResult(redisClient.ContainsKey(key.ToString()));
            }
        }
        public async Task<T> GetAsync<T>(CacheKey key) where T : class, IModel
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                string json = redisClient.GetValue(key.ToString());
                return await Task.FromResult(json.JsonToModel<T>());
            }
        }

        public async Task<T> GetAsync<T>(CacheKey Key, Func<T> func, TimeSpan duration) where T : class, IModel
        {
            T t = default(T);
            string json = this.GetValue(Key);
            if (ModelExtensions.JsonIsEmpty(json))
            {
                t = func();
                this.SetValue(Key, t.ModelToJson(), duration);
            }
            else
            {
                t = json.JsonToModel<T>();
            }
            return await Task.FromResult(t);
        }
        public async Task SetAsync<T>(CacheKey Key, T target, TimeSpan duration) where T : class, IModel
        {
            using (IRedisClient redisClient = GetRedisClient())
            {
                await Task.Run(() => redisClient.SetValue(Key.ToString(), target.ModelToJson(), duration));
            }
        }

        #endregion
    }
}
