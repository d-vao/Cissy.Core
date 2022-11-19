using System;
using System.Collections.Generic;
using System.Text;

namespace Cissy.Payment
{
    public interface IPaymentPluginBuilder
    {
        IPaymentPlugin Build();
    }
    internal class PaymentPluginBuilder<T> : IPaymentPluginBuilder where T : IPaymentPlugin, new()
    {
        public IPaymentPlugin Build()
        {
            return new T();
        }
    }
}
