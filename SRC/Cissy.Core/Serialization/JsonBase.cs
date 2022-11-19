using System;
using System.Collections.Generic;
using System.Text;

namespace Cissy.Serialization
{
    public class JsonBase<T> : IModel
    {
        public JsonBase()
        {

        }
        /// <summary>
        /// 状态值
        /// </summary>
        public int state { get; set; }
        /// <summary>
        /// 结果描述
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public uint timestamp { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public T data { get; set; }
    }
    public class Err : JsonBase<string>
    {
        public Err(int State, string Data = "", string Message = "")
        {
            this.state = State;
            this.data = data;
            this.message = Message;
        }
    }
    public class Success<T> : JsonBase<T>
    {
        public Success(T Data = default(T), string Message = "")
        {
            this.state = 0;
            this.message = Message;
            this.data = Data;
        }
    }
}
