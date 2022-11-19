
using System;
using System.Threading.Tasks;
using Cissy;
using Cissy.Http;
using Cissy.WeiXin.Https;
using Cissy.WeiXin;


namespace Cissy.WeiXin
{
    /// <summary>
    /// 通用接口
    /// 通用接口用于和微信服务器通讯，一般不涉及自有网站服务器的通讯
    /// </summary>
    internal partial class DefaultWeiXinMpApi
    {
        #region 同步方法


        /// <summary>
        /// 获取调用微信JS接口的临时票据
        /// </summary>
        /// <param name="accessTokenOrAppId">AccessToken或AppId（推荐使用AppId，需要先注册）</param>
        /// <param name="type">默认为jsapi，当作为卡券接口使用时，应当为wx_card</param>
        /// <returns></returns>
        public async Task<JsApiTicketResult> GetTicketAsync(string type = "jsapi")
        {
            var url = WeiXinApiHelper.BaseUrl + "/cgi-bin/ticket/getticket?access_token={0}" + $"&type={type}";
            return await CommonJsonSend.SendAsync<JsApiTicketResult, StringRef>(await this.GetAccessTokenAsync(), url, null, CommonJsonSendType.GET);
        }

        /// <summary>
        /// 获取微信服务器的ip段
        /// </summary>
        /// <param name="accessTokenOrAppId">AccessToken或AppId（推荐使用AppId，需要先注册）</param>
        /// <returns></returns>
        public async Task<GetCallBackIpResult> GetCallBackIpAsync()
        {
            var url = WeiXinApiHelper.BaseUrl + "/cgi-bin/getcallbackip?access_token={0}";
            return await CommonJsonSend.SendAsync<GetCallBackIpResult, StringRef>(await this.GetAccessTokenAsync(), url, null, CommonJsonSendType.GET);
        }
        /// <summary>
        ///公众号调用或第三方平台帮公众号调用对公众号的所有api调用（包括第三方帮其调用）次数进行清零
        /// </summary>
        /// <param name="accessTokenOrAppId">AccessToken或AppId（推荐使用AppId，需要先注册）</param>
        /// <param name="appId"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public async Task<WxJsonResult> Clear_quotaAsync()
        {
            var url = WeiXinApiHelper.BaseUrl + "/cgi-bin/clear_quota?access_token={0}";
            return await CommonJsonSend.SendAsync<StringRef>(await this.GetAccessTokenAsync(), url, null, CommonJsonSendType.GET);
        }

        #endregion

      

    }
}
