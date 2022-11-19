using System;
using System.Collections.Generic;
using System.Text;

namespace Cissy.Infrastructure
{
    /// <summary>
    /// 服务构件
    /// </summary>
    public interface IPart
    {
        /// <summary>
        /// 1.初始化
        /// </summary>
        /// <param name=""></param>
        void Init(IPartInitArg Arg);
        /// <summary>
        /// 2.开始
        /// </summary>
        void Start();
        /// <summary>
        /// 3.关闭
        /// </summary>
        void Shut();
        /// <summary>
        /// 4.处理地铁消息
        /// </summary>
        /// <param name="message"></param>
        void HandleMetroMessage(IMetroMessage message);
    }
}
