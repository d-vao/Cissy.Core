using System;
using System.Collections.Generic;
using System.Text;

namespace Cissy.Rebate
{
    /// <summary>
    /// 余额请求
    /// </summary>
    public interface IBalanceRequest : IModel
    {
        /// <summary>
        /// 应用名称
        /// </summary>
        string AppName { get; }
        /// <summary>
        /// 消费者用户Id
        /// </summary>
        string ConsumerId { get; }
        /// <summary>
        /// 商户ID
        /// </summary>
        long MerchantId { get; }
    }
}
