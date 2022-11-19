using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Cissy.Configuration;

namespace Cissy.Payment
{
    public class DefaultPayApp : IPayApp
    {
        PaymentConfig _config;
        public DefaultPayApp(PaymentConfig config)
        {
            _config = config;
        }
        public string GetPayUrl(PaymentRequest request)
        {
            var arg = Actor.Public.Map<PaymentArg>(request);
            arg.AppName = _config.AppName;
            var tokenApp = arg.ToPaymentArgToken(_config.AppSecret);
            if (tokenApp.IsNull())
                return string.Empty;
            return _config.PayUrl + $"/api/{arg.ApiVersion}/pay/do?token={tokenApp.Token}&appname={tokenApp.AppName}";
        }
      
    }
}
