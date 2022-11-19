using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Cissy.Configuration;
using AutoMapper;

namespace Cissy.Payment
{
    /// <summary>
    /// 支付配置帮助
    /// </summary>
    public static class PaymentConfigHelper
    {
        /// <summary>
        /// 注入支付中心服务
        /// </summary>
        /// <param name="cissyConfigBuilder"></param>
        /// <returns></returns>
        public static CissyConfigBuilder AddPaymentCenterConfig(this CissyConfigBuilder cissyConfigBuilder, Action<PaymentChannelRegister> action)
        {
            action(new PaymentChannelRegister());
            return cissyConfigBuilder;
        }
        /// <summary>
        /// 注入支付服务
        /// </summary>
        /// <param name="cissyConfigBuilder"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static CissyConfigBuilder AddPaymentConfig(this CissyConfigBuilder cissyConfigBuilder)
        {
            ICissyConfig cissyConfig = cissyConfigBuilder.CissyConfig;
            if (cissyConfig.IsNotNull())
            {
                PaymentConfig payConfig = cissyConfig.GetConfig<PaymentConfig>();
                if (payConfig.IsNotNull())
                {
                    var factory = new DefaultPayAppFactory(payConfig);
                    cissyConfigBuilder.ServiceCollection.AddSingleton(typeof(IPayAppFactory), factory);
                }
            }
            cissyConfigBuilder.RegisterMapper(register =>
            {
                register.CreateMap<PaymentRequest, PaymentArg>().ForMember(b => b.AppName, n => n.Ignore());
            });
            return cissyConfigBuilder;
        }
    }
}
