using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Cissy.Serialization;
using System.Security.Cryptography;
using System.Security.Claims;
using Cissy.Authentication.JWT;

namespace Cissy.Payment
{
    public class PayNotifyClaimTypes
    {
        public const string Amount = "amount";
        public const string OrderIds = "orderids";
        public const string TradNo = "tradno";
        public const string ChannelName = "channelname";
        public const string ResponseContentWhenFinished = "responsecontentwhenfinished";
        public const string TradeTime = "tradetime";
    }
    public class PayNotify
    {
        public int Amount { get; set; }
        public string OrderIds { get; set; }
        public string TradNo { get; set; }
        public string ChannelName { get; set; }
        public string ResponseContentWhenFinished { get; set; }
        public uint TradeTime { get; set; }
    }
    public static class PayNotifyHelper
    {
        public static string PayNotifyToToken(this PayNotify arg, string secret)
        {
            List<Claim> list = new List<Claim>();
            Claim Amount = new Claim(PayNotifyClaimTypes.Amount, arg.Amount.ToString());
            list.Add(Amount);
            Claim OrderIds = new Claim(PayNotifyClaimTypes.OrderIds, arg.OrderIds);
            list.Add(OrderIds);
            Claim TradNo = new Claim(PayNotifyClaimTypes.TradNo, arg.TradNo);
            list.Add(TradNo);
            Claim ChannelName = new Claim(PayNotifyClaimTypes.ChannelName, arg.ChannelName);
            list.Add(ChannelName);
            Claim ResponseContentWhenFinished = new Claim(PayNotifyClaimTypes.ResponseContentWhenFinished, arg.ResponseContentWhenFinished);
            list.Add(ResponseContentWhenFinished);
            Claim TradeTime = new Claim(PayNotifyClaimTypes.TradeTime, arg.TradeTime.ToString());
            list.Add(TradeTime);

            DefaultJwtPrincipalBuilder jwtBuilder = new DefaultJwtPrincipalBuilder(secret);
            return jwtBuilder.BuildToken(list);
        }
        public static PayNotify PayNotifyFromToken(this string token, string secret)
        {
            DefaultJwtPrincipalBuilder builder = new DefaultJwtPrincipalBuilder(secret);
            var claimsPrincipal = builder.GetPrincipal("paynotifytoken", token);
            if (claimsPrincipal.IsNull())
            {
                return default;
            }
            PayNotify arg = new PayNotify();
            arg.Amount = int.Parse(claimsPrincipal.Claims.FirstOrDefault(m => m.Type == PayNotifyClaimTypes.Amount).Value);
            arg.OrderIds = claimsPrincipal.Claims.FirstOrDefault(m => m.Type == PayNotifyClaimTypes.OrderIds).Value;
            arg.TradNo = claimsPrincipal.Claims.FirstOrDefault(m => m.Type == PayNotifyClaimTypes.TradNo).Value;
            arg.ChannelName = claimsPrincipal.Claims.FirstOrDefault(m => m.Type == PayNotifyClaimTypes.ChannelName).Value;
            arg.ResponseContentWhenFinished = claimsPrincipal.Claims.FirstOrDefault(m => m.Type == PayNotifyClaimTypes.ResponseContentWhenFinished).Value;
            arg.TradeTime = uint.Parse(claimsPrincipal.Claims.FirstOrDefault(m => m.Type == PayNotifyClaimTypes.TradeTime).Value);
            return arg;
        }
    }
}
