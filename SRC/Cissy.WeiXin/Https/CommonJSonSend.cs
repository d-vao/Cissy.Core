using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Cissy.Http;
using Newtonsoft.Json;
using Cissy.Serialization.Json;

namespace Cissy.WeiXin.Https
{
    /// <summary>
    /// CommonJsonSend
    /// </summary>
    internal static class CommonJsonSend
    {
        #region 同步请求

        /// <summary>
        /// 向需要AccessToken的API发送消息的公共方法
        /// </summary>
        /// <param name="accessToken">这里的AccessToken是通用接口的AccessToken，非OAuth的。如果不需要，可以为null，此时urlFormat不要提供{0}参数</param>
        /// <param name="urlFormat"></param>
        /// <param name="data">如果是Get方式，可以为null</param>
        /// <param name="sendType"></param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <param name="jsonSetting"></param>
        /// <returns></returns>
        //public static WxJsonResult Send<T>(string accessToken, string urlFormat, T data, CommonJsonSendType sendType = CommonJsonSendType.POST) where T : IModel
        //{
        //    return SendAsync<WxJsonResult, T>(accessToken, urlFormat, data, sendType);
        //}

        /// <summary>
        /// 向需要AccessToken的API发送消息的公共方法
        /// </summary>
        /// <param name="accessToken">这里的AccessToken是通用接口的AccessToken，非OAuth的。如果不需要，可以为null，此时urlFormat不要提供{0}参数</param>
        /// <param name="urlFormat">用accessToken参数填充{0}</param>
        /// <param name="data">如果是Get方式，可以为null</param>
        /// <param name="sendType"></param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <param name="checkValidationResult"></param>
        /// <param name="jsonSetting"></param>
        /// <returns></returns>
        //public static async Task<T> SendAsync<T, K>(string accessToken, string urlFormat, K data, CommonJsonSendType sendType = CommonJsonSendType.POST) where T : IModel where K : IModel
        //{
        //    //TODO:此方法可以设定一个日志记录开关
        //    var url = string.IsNullOrEmpty(accessToken) ? urlFormat : string.Format(urlFormat, accessToken);
        //    switch (sendType)
        //    {
        //        case CommonJsonSendType.GET:
        //            T getT = default;
        //            await Actor.Public.GetAsync(url, m =>
        //                         {
        //                             getT = m.JsonToModel<T>();
        //                         });
        //            return getT;
        //        case CommonJsonSendType.POST:
        //            T postT = default(T);
        //            StringContent content = new StringContent(data.ModelToJson());
        //            await Actor.Public.PostAsync(url, content, m =>
        //                             {
        //                                 postT = m.JsonToModel<T>();
        //                             });
        //            return postT;
        //        //TODO:对于特定的错误类型自动进行一次重试，如40001（目前的问题是同样40001会出现在不同的情况下面）
        //        default:
        //            throw new ArgumentOutOfRangeException("sendType");
        //    }
        //}
        #endregion
        #region 异步方法

        /// <summary>
        /// 向需要AccessToken的API发送消息的公共方法
        /// </summary>
        /// <param name="accessToken">这里的AccessToken是通用接口的AccessToken，非OAuth的。如果不需要，可以为null，此时urlFormat不要提供{0}参数</param>
        /// <param name="urlFormat"></param>
        /// <param name="data">如果是Get方式，可以为null</param>
        /// <param name="sendType"></param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <param name="checkValidationResult">验证服务器证书回调自动验证</param>
        /// <param name="jsonSetting"></param>
        /// <returns></returns>
        public static async Task<WxJsonResult> SendAsync<T>(string accessToken, string urlFormat, T data, CommonJsonSendType sendType = CommonJsonSendType.POST) where T : IModel
        {
            return await SendAsync<WxJsonResult, T>(accessToken, urlFormat, data, sendType);
        }

        /// <summary>
        /// 向需要AccessToken的API发送消息的公共方法
        /// </summary>
        /// <param name="accessToken">这里的AccessToken是通用接口的AccessToken，非OAuth的。如果不需要，可以为null，此时urlFormat不要提供{0}参数</param>
        /// <param name="urlFormat"></param>
        /// <param name="data">如果是Get方式，可以为null。在POST方式中将被转为JSON字符串提交</param>
        /// <param name="sendType">发送类型，POST或GET，默认为POST</param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <param name="checkValidationResult">验证服务器证书回调自动验证</param>
        /// <param name="jsonSetting">JSON字符串生成设置</param>
        /// <returns></returns>
        public static async Task<T> SendAsync<T, K>(string accessToken, string urlFormat, K data,
            CommonJsonSendType sendType = CommonJsonSendType.POST) where T : IModel where K : IModel
        {

            var url = string.IsNullOrEmpty(accessToken) ? urlFormat : string.Format(urlFormat, accessToken);
            switch (sendType)
            {
                case CommonJsonSendType.GET:
                    T getT = default(T);
                    await Actor.Public.GetAsync(url, m =>
                     {
                         getT = m.JsonToModel<T>();
                     });
                    return getT;
                case CommonJsonSendType.POST:
                    T postT = default(T);
                    StringContent content = new StringContent(data.ModelToJson());
                    await Actor.Public.PostAsync(url, content, m =>
                     {
                         postT = m.JsonToModel<T>();
                     });
                    return postT;
                default:
                    throw new ArgumentOutOfRangeException("sendType");
            }

        }

        #endregion
    }

}


