using System;
using System.Collections.Generic;

namespace Cissy.Payment
{
    /// <summary>
    /// 支付事件
    /// </summary>
    public abstract class PaymentInfoBase
    {
        /// <summary>
        /// 支付金额,单位：分
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// 支付订单号
        /// </summary>
        public string OrderIds { get; set; }

        /// <summary>
        /// 支付流水号
        /// </summary>
        public string TradNo { get; set; }
        /// <summary>
        /// 主业务处理完成后响应内容
        /// 即当主程序相关订单状态完成后，需要响应请求的内容
        /// </summary>
        public string ResponseContentWhenFinished { get; set; }

    }
    public class PaymentInfo : PaymentInfoBase
    {
        /// <summary>
        /// 交易时间
        /// </summary>
        public DateTime? TradeTime { get; set; }
        public string AttachData { get; set; } = PaymentArg.DefaultAttachData;

    }
    public class PaymentEvent : PaymentInfoBase
    {
        /// <summary>
        /// 交易时间
        /// </summary>
        public uint TradeTime { get; set; }

    }
}
