using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using Cissy.Plugins;
using Microsoft.AspNetCore.Http;

namespace Cissy.Payment
{
    /// <summary>
    /// 支付插件接口
    /// </summary>
    public interface IPaymentPlugin : IPlugin
    {
        PayTie Tie { get; }
        bool NeedRedirect { get; }
        string ChannelName { get; }
        /// <summary>
        /// 处理支付返回结果
        /// </summary>
        /// <param name="context">请求</param>
        /// <exception cref="ApplicationException"></exception>
        /// <returns></returns>
        PaymentInfo ProcessReturn(HttpContext context);

        /// <summary>
        /// 处理异步通知结果
        /// </summary>
        /// <param name="queryString">请求参数</param>
        /// <exception cref="ApplicationException"></exception>
        /// <returns></returns>
        PaymentInfo ProcessNotify(HttpContext context, IPaymentConfig config);

        /// <summary>
        /// 2017-10-19填加的新支付接口方法
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <param name="notifyUrl"></param>
        /// <param name="orderId"></param>
        /// <param name="totalFee"></param>
        /// <param name="productInfo"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        string GetRequestUrl(HttpContext context, IPaymentConfig config, PaymentArg arg, string returnUrl, string notifyUrl);
        /// <summary>
        /// 退款处理
        /// </summary>
        /// <param name="para">退款所需参数</param>
        /// <returns>退款成功时，返回不为空</returns>
        RefundFeeReturnModel ProcessRefundFee(IPaymentConfig config, PaymentPara para);
        /// <summary>
        /// 处理退款返回结果
        /// </summary>
        /// <param name="context">请求</param>
        /// <exception cref="ApplicationException"></exception>
        /// <returns></returns>
        PaymentInfo ProcessRefundReturn(IPaymentConfig config, HttpContext context);

        /// <summary>
        /// 处理退款异步通知结果
        /// </summary>
        /// <param name="queryString">请求参数</param>
        /// <exception cref="ApplicationException"></exception>
        /// <returns></returns>
        PaymentInfo ProcessRefundNotify(HttpContext context);
        /// <summary>
        /// 企业付款
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        PaymentInfo EnterprisePay(IPaymentConfig config, EnterprisePayPara para);



        ///// <summary>
        ///// 获取表单数据
        ///// </summary>
        //FormData GetFormData();

        ///// <summary>
        ///// 设置表单数据
        ///// </summary>
        ///// <param name="values">表单数据键值对集合，键为表单项的name,值为用户填写的值</param>
        //void SetFormValues(IEnumerable<KeyValuePair<string, string>> values);

        /// <summary>
        /// 确认收到支付消息，获取待返回至第三方支付平台的内容
        /// </summary>
        /// <returns></returns>
        string ConfirmPayResult();



        /// <summary>
        /// 支付请求链接类型
        /// </summary>
        UrlType RequestUrlType { get; }


    }
}
