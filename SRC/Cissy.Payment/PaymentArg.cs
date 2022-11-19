using System;
using System.Collections.Generic;
using System.Text;
using Cissy;
using Cissy.Serialization;
using System.IO;

namespace Cissy.Payment
{
    public static class PayargClaimTypes
    {
        public const string OrderId = "orderid";
        public const string Amount = "amount";
        public const string MerchantId = "merchantid";
        public const string OrderInfo = "orderinfo";
        public const string AttachData = "attachdata";
        public const string ChannelName = "channelname";
        public const string OpenId = "openid";
        public const string AppId = "appid";
        public const string ReturnUrl = "returnurl";
        public const string BackUrl = "backurl";
        public const string ApiVersion = "apiversion";
        public const string AppName = "appname";
    }
    public partial class PaymentRequest : IModel
    {
        public const string NullReturnUrl = "-";
        public const string NullBackUrl = "-";
        public const string NullOpenId = "*";
        public const string NullAppId = "*";
        public const string DefaultApiVersion = "1.0";
        public const string DefaultAttachData = "*";
        public string BizOrderId { get; set; }
        public int Amount { get; set; }
        public string OrderInfo { get; set; }
        public string AttachData { get; set; }
        public string ChannelName { get; set; }
        public long MerchantId { get; set; }
        public string OpenId { get; set; }
        public string AppId { get; set; }
        public string ReturnUrl { get; set; }
        public string BackUrl { get; set; }
        public string ApiVersion { get; set; }
        internal PaymentRequest()
        {
            this.BizOrderId = string.Empty;
            this.OrderInfo = string.Empty;
            this.AttachData = string.Empty;
            this.ChannelName = string.Empty;
            this.OpenId = NullOpenId;
            this.AppId = NullAppId;
            this.ReturnUrl = ReturnUrl;
            this.BackUrl = NullBackUrl;
            this.ApiVersion = DefaultApiVersion;
        }
        public static PaymentRequest Create(int amount, string channelName, long merchantId, string bizOrderId, string orderInfo, string attachData = DefaultAttachData, string ApiVersion = DefaultApiVersion, string returnUrl = NullReturnUrl, string backUrl = NullBackUrl, string openId = NullOpenId, string appId = NullAppId)
        {
            PaymentRequest request = new PaymentRequest();
            request.Amount = amount;
            request.MerchantId = merchantId;
            request.ChannelName = channelName;
            request.BizOrderId = bizOrderId;
            request.OrderInfo = orderInfo;
            request.AttachData = attachData;
            request.ReturnUrl = returnUrl;
            request.BackUrl = backUrl;
            request.OpenId = openId;
            request.AppId = appId;
            request.ApiVersion = ApiVersion;
            return request;
        }
    }
    /// <summary>
    /// 支付参数
    /// </summary>
    public partial class PaymentArg : PaymentRequest, ISerializable
    {

        public PaymentArg() : base()
        {
            this.AppName = string.Empty;
        }
        public string AppName { get; set; }
        public virtual int Size => BizOrderId.GetVarSize()
            + sizeof(int)
            + OrderInfo.GetVarSize()
            + AttachData.GetVarSize()
            + ChannelName.GetVarSize()
            + sizeof(long)
            + OpenId.GetVarSize()
            + AppId.GetVarSize()
            + ReturnUrl.GetVarSize()
            + BackUrl.GetVarSize()
            + ApiVersion.GetVarSize()
            + AppName.GetVarSize();

        public void Deserialize(BinaryReader reader)
        {
            BizOrderId = reader.ReadVarString();
            Amount = reader.ReadInt32();
            OrderInfo = reader.ReadVarString();
            AttachData = reader.ReadVarString();
            ChannelName = reader.ReadVarString();
            MerchantId = reader.ReadInt64();
            OpenId = reader.ReadVarString();
            AppId = reader.ReadVarString();
            ReturnUrl = reader.ReadVarString();
            BackUrl = reader.ReadVarString();
            ApiVersion = reader.ReadVarString();
            AppName = reader.ReadVarString();
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.WriteVarString(BizOrderId);
            writer.Write(Amount);
            writer.WriteVarString(OrderInfo);
            writer.WriteVarString(AttachData);
            writer.WriteVarString(ChannelName);
            writer.Write(MerchantId);
            writer.WriteVarString(OpenId);
            writer.WriteVarString(AppId);
            writer.WriteVarString(ReturnUrl);
            writer.WriteVarString(BackUrl);
            writer.WriteVarString(ApiVersion);
            writer.WriteVarString(AppName);
        }
    }
}
