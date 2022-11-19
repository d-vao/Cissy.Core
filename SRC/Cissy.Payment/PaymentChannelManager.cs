using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Cissy.Payment
{
    public delegate void RegisterChannel<T>(string ChannelName) where T : IPaymentPlugin, new();
    public class PaymentChannelRegister
    {

    }
    public static class PaymentChannelManager
    {
        static ConcurrentDictionary<string, IPaymentPluginBuilder> Channels = new ConcurrentDictionary<string, IPaymentPluginBuilder>();
        public static void RegisterChannel<T>(this PaymentChannelRegister Register, string ChannelName) where T : IPaymentPlugin, new()
        {
            Channels[ChannelName] = new PaymentPluginBuilder<T>();
        }
        public static IPaymentPlugin GetPaymentPlugin(this Public Public, string ChannelName)
        {
            if (Channels.TryGetValue(ChannelName, out IPaymentPluginBuilder builder))
            {
                return builder.Build();
            }
            return default(IPaymentPlugin);
        }
    }
}
