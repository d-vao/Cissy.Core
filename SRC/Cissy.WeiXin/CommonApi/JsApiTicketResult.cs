using System;
using Cissy.WeiXin.Https;

namespace Cissy.WeiXin
{
    /// <summary>
    /// jsapi_ticket请求后的JSON返回格式
    /// </summary>
    [Serializable]
    public class JsApiTicketResult : WxJsonResult
    {
        /// <summary>
        /// 获取到的凭证
        /// </summary>
        public string ticket { get; set; }
        /// <summary>
        /// 凭证有效时间，单位：秒
        /// </summary>
        public int expires_in { get; set; }
    }
}
