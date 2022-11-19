using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using Cissy.Redis;
using ServiceStack.Redis;

namespace Cissy.Redis
{
    public class WebRedisCluster : RedisCluster
    {
        /// <summary>
        /// 注册消息
        /// </summary>
        public override void RegisterMessage(ConcurrentDictionary<string, MessageHandler> MessageTypes)
        {
        }
        /// <summary>
        /// 注册消息事件
        /// </summary>
        /// <param name="MessageType"></param>
        public override void OnRegisterMessage(string MessageType) { }
        /// <summary>
        /// 注销消息事件 
        /// </summary>
        /// <param name="MessageType"></param>
        public override void OnUnRegisterMessage(string MessageType) { }
        public string DestMessageType(long ServerID, string MessageType)
        {
            return string.Format("{0}#{1}", ServerID, MessageType);
        }
        //public void BroadAD(Passport passport)
        //{
        //    using (IRedisClient redisClient = GetRedisClient())
        //    {
        //        var result = redisClient.PublishMessage(DestMessageType(passport.ServerID, RedisSubscribeChannel.BroadAD), JsonConvert.SerializeObject(passport));
        //    }
        //}
    }
}
