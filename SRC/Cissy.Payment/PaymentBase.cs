using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cissy.Plugins;
using Microsoft.AspNetCore.Http;

namespace Cissy.Payment
{
    public class PaymentBase
    {
        /// <summary>
        /// 退款入口
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public virtual RefundFeeReturnModel ProcessRefundFee(IPaymentConfig config, PaymentPara para)
        {
            throw new PluginException("未实现此方法");
        }
        /// <summary>
        /// 处理退款返回结果
        /// </summary>
        /// <param name="context">请求</param>
        /// <exception cref="ApplicationException"></exception>
        /// <returns></returns>
        public virtual PaymentInfo ProcessRefundReturn(IPaymentConfig config, HttpContext context)
        {
            throw new PluginException("未实现此方法");
        }

        /// <summary>
        /// 处理退款异步通知结果
        /// </summary>
        /// <param name="queryString">请求参数</param>
        /// <exception cref="ApplicationException"></exception>
        /// <returns></returns>
        public virtual PaymentInfo ProcessRefundNotify(HttpContext context)
        {
            throw new PluginException("未实现此方法");
        }

        public virtual PaymentInfo EnterprisePay(IPaymentConfig config, EnterprisePayPara para)
        {
            throw new PluginException("未实现此方法");
        }
    }
}
