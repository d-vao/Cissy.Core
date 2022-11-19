/*----------------------------------------------------------------
    Copyright (C) 2015 Senparc
    
    文件名：Get.cs
    文件功能描述：Get
    
    
    创建标识：Senparc - 20150211
    
    修改标识：Senparc - 20150303
    修改描述：整理接口
----------------------------------------------------------------*/

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Cissy.Serialization.Json;

namespace Cissy.WeiXin.Https
{
    internal static class Get
    {
        #region 同步方法
        /// <summary>
        /// GET方式请求URL，并返回T类型
        /// </summary>
        /// <typeparam name="T">接收JSON的数据类型</typeparam>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static T GetJson<T>(string url, Encoding encoding = null) where T : IModel
        {
            string returnText = RequestUtility.HttpGet(url, encoding);

            //WeixinTrace.SendLog(url, returnText);


            if (returnText.Contains("errcode"))
            {
                //可能发生错误
                WxJsonResult errorResult = returnText.GetObject<WxJsonResult>();
                if (errorResult.errcode != ReturnCode.请求成功)
                {
                    //发生错误
                    throw new ApplicationException(
                        string.Format("微信请求发生错误！错误代码：{0}，说明：{1}",
                                        (int)errorResult.errcode, errorResult.errmsg));
                }
            }

            T result = returnText.GetObject<T>();

            return result;
        }

        /// <summary>
        /// 从Url下载
        /// </summary>
        /// <param name="url"></param>
        /// <param name="stream"></param>
        public static void Download(string url, Stream stream)
        {
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);  

            WebClient wc = new WebClient();
            var data = wc.DownloadData(url);
            foreach (var b in data)
            {
                stream.WriteByte(b);
            }
        }

        #endregion

    }
}
