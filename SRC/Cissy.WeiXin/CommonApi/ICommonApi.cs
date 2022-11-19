using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cissy.WeiXin.Https;

namespace Cissy.WeiXin
{
    public interface ICommonApi
    {
        /// <summary>
        /// 获取调用微信JS接口的临时票据
        /// </summary>
        /// <param name="accessTokenOrAppId">AccessToken或AppId（推荐使用AppId，需要先注册）</param>
        /// <param name="type">默认为jsapi，当作为卡券接口使用时，应当为wx_card</param>
        /// <returns></returns>
        Task<JsApiTicketResult> GetTicketAsync(string type = "jsapi");
        /// <summary>
        /// 获取微信服务器的ip段
        /// </summary>
        /// <param name="accessTokenOrAppId">AccessToken或AppId（推荐使用AppId，需要先注册）</param>
        /// <returns></returns>
        Task<GetCallBackIpResult> GetCallBackIpAsync();
        /// <summary>
        ///公众号调用或第三方平台帮公众号调用对公众号的所有api调用（包括第三方帮其调用）次数进行清零
        /// </summary>
        /// <param name="accessTokenOrAppId">AccessToken或AppId（推荐使用AppId，需要先注册）</param>
        /// <param name="appId"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
       Task< WxJsonResult> Clear_quotaAsync();

       
    }
}
