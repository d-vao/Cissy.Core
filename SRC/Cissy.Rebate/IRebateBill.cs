using System;
using System.Collections.Generic;
using System.Text;

namespace Cissy.Rebate
{
    /// <summary>
    /// 返利结算单
    /// </summary>
    public interface IRebateBill : IModel
    {
        /// <summary>
        /// 应用名称
        /// </summary>
        string AppName { get; }
        /// <summary>
        /// 消费者用户Id
        /// </summary>
        string ConsumerId { get; }
    }
}
