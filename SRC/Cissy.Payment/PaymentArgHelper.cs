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
    public class PaymentAppToken
    {
        public string Token;
        public string AppName;
    }
    public enum PaymentArgResultStatus
    {
        /// <summary>
        /// 错误
        /// </summary>
        Faild = 1,
        /// <summary>
        /// 跳转
        /// </summary>
        Redirect = 2,
        /// <summary>
        /// 成功
        /// </summary>
        Success = 3
    }
    public class PaymentArgResult
    {
        public PaymentArgResult(PaymentArgResultStatus state)
        {
            this.State = state;
        }
        public PaymentArgResultStatus State { get; private set; }
        public PayTie Tie { get; set; }
        public UrlType UrlType { get; set; }
    }
    public static class PaymentArgHelper
    {
        public static string ToUrlCode(this PaymentArg arg)
        {
            var bs = arg.ToArray();
            return System.Web.HttpUtility.UrlEncode(bs);
        }
        public static PaymentArg FromUrlCode(this string code)
        {
            var bs = System.Web.HttpUtility.UrlDecodeToBytes(code, Encoding.UTF8);
            return SeriaHelper.DeserializeFrom<PaymentArg>(bs);
        }
        public static PaymentAppToken ToPaymentArgToken(this PaymentArg arg, string secret)
        {
            List<Claim> list = new List<Claim>();
            Claim orderid = new Claim(PayargClaimTypes.OrderId, arg.BizOrderId);
            list.Add(orderid);
            Claim Amount = new Claim(PayargClaimTypes.Amount, arg.Amount.ToString());
            list.Add(Amount);
            Claim ApiVersion = new Claim(PayargClaimTypes.ApiVersion, arg.ApiVersion);
            list.Add(ApiVersion);
            Claim AppName = new Claim(PayargClaimTypes.AppName, arg.AppName);
            list.Add(AppName);
            Claim AttachData = new Claim(PayargClaimTypes.AttachData, arg.AttachData);
            list.Add(AttachData);
            Claim BackUrl = new Claim(PayargClaimTypes.BackUrl, arg.BackUrl);
            list.Add(BackUrl);
            Claim ChannelName = new Claim(PayargClaimTypes.ChannelName, arg.ChannelName);
            list.Add(ChannelName);
            Claim MerchantId = new Claim(PayargClaimTypes.MerchantId, arg.MerchantId.ToString());
            list.Add(MerchantId);
            Claim OpenId = new Claim(PayargClaimTypes.OpenId, arg.OpenId);
            list.Add(OpenId);
            Claim AppId = new Claim(PayargClaimTypes.AppId, arg.AppId);
            list.Add(AppId);
            Claim OrderInfo = new Claim(PayargClaimTypes.OrderInfo, arg.OrderInfo);
            list.Add(OrderInfo);
            Claim ReturnUrl = new Claim(PayargClaimTypes.ReturnUrl, arg.ReturnUrl);
            list.Add(ReturnUrl);
            DefaultJwtPrincipalBuilder jwtBuilder = new DefaultJwtPrincipalBuilder(secret);
            var token = jwtBuilder.BuildToken(list);

            PaymentAppToken pac = new PaymentAppToken()
            {
                Token = token,
                AppName = arg.AppName
            };
            //var bs = arg.ToArray();
            //pac.Code = System.Web.HttpUtility.UrlEncode(bs);
            //var symmetricKey = Convert.FromBase64String(secret);
            //HMACSHA256 h256 = new HMACSHA256(symmetricKey);
            //pac.Signature = System.Web.HttpUtility.UrlEncode(Convert.ToBase64String(h256.ComputeHash(bs)));
            return pac;
        }
        //public static PaymentArgResult FromUrlEncryptCode(this PaymentArgCode code, out PaymentArg arg, Func<PaymentArg, string> GetSecret)
        //{
        //    var bs = System.Web.HttpUtility.UrlDecodeToBytes(code.Code);
        //    arg = SeriaHelper.DeserializeFrom<PaymentArg>(bs);
        //    if (arg.IsNull())
        //        return new PaymentArgResult(PaymentArgResultStatus.Faild);
        //    var plugin = Actor.Public.GetPaymentPlugin(arg.ChannelName);
        //    if (plugin.IsNull())
        //    {
        //        arg = default(PaymentArg);
        //        return new PaymentArgResult(PaymentArgResultStatus.Faild);
        //    }
        //    if (plugin.NeedRedirect)
        //    {
        //        return new PaymentArgResult(PaymentArgResultStatus.Redirect) { Tie = plugin.Tie, UrlType = plugin.RequestUrlType };
        //    }
        //    var secret = GetSecret(arg);
        //    if (secret.IsNullOrEmpty())
        //    {
        //        arg = default(PaymentArg);
        //        return new PaymentArgResult(PaymentArgResultStatus.Faild) { Tie = plugin.Tie, UrlType = plugin.RequestUrlType };
        //    }
        //    var symmetricKey = Convert.FromBase64String(secret);
        //    HMACSHA256 h256 = new HMACSHA256(symmetricKey);
        //    var b64 = Convert.ToBase64String(h256.ComputeHash(bs));
        //    //var sign = System.Web.HttpUtility.UrlEncode(b64);
        //    if (code.Signature != b64)
        //        return new PaymentArgResult(PaymentArgResultStatus.Faild) { Tie = plugin.Tie, UrlType = plugin.RequestUrlType };
        //    return new PaymentArgResult(PaymentArgResultStatus.Success) { Tie = plugin.Tie, UrlType = plugin.RequestUrlType };
        //}

        public static PaymentArgResult FromPayToken(this PaymentAppToken apptoken, out PaymentArg arg, Func<string, string> GetSecret)
        {
            var secret = GetSecret(apptoken.AppName);
            if (secret.IsNullOrEmpty())
            {
                arg = default(PaymentArg);
                return new PaymentArgResult(PaymentArgResultStatus.Faild);
            }
            DefaultJwtPrincipalBuilder builder = new DefaultJwtPrincipalBuilder(secret);
            var claimsPrincipal = builder.GetPrincipal("paytoken", apptoken.Token);
            if (claimsPrincipal.IsNull())
            {
                arg = default(PaymentArg);
                return new PaymentArgResult(PaymentArgResultStatus.Faild);
            }
            arg = new PaymentArg();
            arg.Amount = int.Parse(claimsPrincipal.Claims.FirstOrDefault(m => m.Type == PayargClaimTypes.Amount).Value);
            arg.ApiVersion = claimsPrincipal.Claims.FirstOrDefault(m => m.Type == PayargClaimTypes.ApiVersion).Value;
            arg.AppName = claimsPrincipal.Claims.FirstOrDefault(m => m.Type == PayargClaimTypes.AppName).Value;
            arg.AttachData = claimsPrincipal.Claims.FirstOrDefault(m => m.Type == PayargClaimTypes.AttachData).Value;
            arg.BackUrl = claimsPrincipal.Claims.FirstOrDefault(m => m.Type == PayargClaimTypes.BackUrl).Value;
            arg.BizOrderId = claimsPrincipal.Claims.FirstOrDefault(m => m.Type == PayargClaimTypes.OrderId).Value;
            arg.ChannelName = claimsPrincipal.Claims.FirstOrDefault(m => m.Type == PayargClaimTypes.ChannelName).Value;
            arg.MerchantId = long.Parse(claimsPrincipal.Claims.FirstOrDefault(m => m.Type == PayargClaimTypes.MerchantId).Value);
            arg.OpenId = claimsPrincipal.Claims.FirstOrDefault(m => m.Type == PayargClaimTypes.OpenId).Value;
            arg.AppId = claimsPrincipal.Claims.FirstOrDefault(m => m.Type == PayargClaimTypes.AppId).Value;
            arg.OrderInfo = claimsPrincipal.Claims.FirstOrDefault(m => m.Type == PayargClaimTypes.OrderInfo).Value;
            arg.ReturnUrl = claimsPrincipal.Claims.FirstOrDefault(m => m.Type == PayargClaimTypes.ReturnUrl).Value;

            //if (arg.IsNull())
            //    return new PaymentArgResult(PaymentArgResultStatus.Faild);
            var plugin = Actor.Public.GetPaymentPlugin(arg.ChannelName);
            if (plugin.IsNull())
            {
                arg = default(PaymentArg);
                return new PaymentArgResult(PaymentArgResultStatus.Faild);
            }
            if (plugin.NeedRedirect)
            {
                return new PaymentArgResult(PaymentArgResultStatus.Redirect) { Tie = plugin.Tie, UrlType = plugin.RequestUrlType };
            }
            return new PaymentArgResult(PaymentArgResultStatus.Success) { Tie = plugin.Tie, UrlType = plugin.RequestUrlType };
        }
        public static PaymentArgResult GetPaymentArgResult(this PaymentRequest arg)
        {
            if (arg.IsNull())
                return new PaymentArgResult(PaymentArgResultStatus.Faild);
            var plugin = Actor.Public.GetPaymentPlugin(arg.ChannelName);
            if (plugin.IsNull())
            {
                arg = default(PaymentArg);
                return new PaymentArgResult(PaymentArgResultStatus.Faild);
            }
            if (plugin.NeedRedirect)
            {
                return new PaymentArgResult(PaymentArgResultStatus.Redirect) { Tie = plugin.Tie, UrlType = plugin.RequestUrlType };
            }
            return new PaymentArgResult(PaymentArgResultStatus.Success) { Tie = plugin.Tie, UrlType = plugin.RequestUrlType };
        }
    }
}
