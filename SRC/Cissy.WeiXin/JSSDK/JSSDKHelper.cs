

using System;
using System.Linq;
using System.Collections;
using System.Text;
using System.Threading.Tasks;

namespace Cissy.WeiXin.JSSDK
{
    /// <summary>
    /// JS-SDK 帮助类
    /// </summary>
    public class JSSDKHelper
    {
        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <returns></returns>
        public static string GetNoncestr()
        {
            return EncryptHelper.GetMD5(Guid.NewGuid().ToString(), "UTF-8");
        }

       

        /// <summary>
        /// sha1加密
        /// </summary>
        /// <returns></returns>
        private static string CreateSha1(Hashtable parameters)
        {
            var sb = new StringBuilder();
            var akeys = new ArrayList(parameters.Keys);
            akeys.Sort(ASCIISort.Create());

            foreach (var k in akeys)
            {
                if (parameters[k] != null)
                {
                    var v = (string)parameters[k];

                    if (sb.Length == 0)
                    {
                        sb.Append(k + "=" + v);
                    }
                    else
                    {
                        sb.Append("&" + k + "=" + v);
                    }
                }
            }
            return EncryptHelper.GetSha1(sb.ToString());
        }

        /// <summary>
        /// 生成cardSign的加密方法
        /// </summary>
        /// <returns></returns>
        private static string CreateCardSha1(Hashtable parameters)
        {
            var sb = new StringBuilder();
            var akeys = new ArrayList(parameters.Keys);
            akeys.Sort(ASCIISort.Create());

            foreach (var k in akeys)
            {
                if (parameters[k] != null)
                {
                    var v = (string)parameters[k];
                    sb.Append(v);
                }
            }
            return EncryptHelper.GetSha1(sb.ToString()).ToString().ToLower();
        }



        /// <summary>
        /// 添加卡券Ext参数的签名加密方法
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static string CreateNonekeySha1(Hashtable parameters)
        {
            var sb = new StringBuilder();
            var aValues = new ArrayList(parameters.Values);
            aValues.Sort(ASCIISort.Create());
            foreach (var v in aValues)
            {
                sb.Append(v);
            }
            return EncryptHelper.GetSha1(sb.ToString()).ToString().ToLower();
        }

        /// <summary>
        /// 获取JS-SDK权限验证的签名Signature
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="noncestr"></param>
        /// <param name="timestamp"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetSignature(string ticket, string noncestr, string timestamp, string url)
        {
            var parameters = new Hashtable();
            parameters.Add("jsapi_ticket", ticket);
            parameters.Add("noncestr", noncestr);
            parameters.Add("timestamp", timestamp);
            parameters.Add("url", url);
            return CreateSha1(parameters);
        }

        /// <summary>
        /// 获取位置签名AddrSign
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="oauthAccessToken">必须是OAuth的AccessToken</param>
        /// <param name="noncestr"></param>
        /// <param name="timestamp"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetAddrSign(string appId, string oauthAccessToken, string noncestr, string timestamp,
            string url)
        {
            //TODO:此处的accessToken应该为OAuth的AccessToken
            //var accessToken = AccessTokenContainer.TryGetAccessToken(appId, appSecret);
            var parameters = new Hashtable();
            parameters.Add("appid", appId);
            parameters.Add("noncestr", noncestr);
            parameters.Add("timestamp", timestamp);
            parameters.Add("url", url);
            parameters.Add("accesstoken", oauthAccessToken);
            return CreateSha1(parameters);
        }

        /// <summary>
        /// 获取卡券签名CardSign
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="locationId"></param>
        /// <param name="noncestr"></param>
        /// <param name="timestamp"></param>
        /// <param name="cardId"></param>
        /// <param name="cardType"></param>
        /// <returns></returns>
        public static string GetCardSign(string appId, string appSecret, string locationId, string noncestr,
            string timestamp, string cardId, string cardType)
        {
            var parameters = new Hashtable();
            parameters.Add("appId", appId);
            parameters.Add("appsecret", appSecret);
            parameters.Add("location_id", locationId);
            parameters.Add("nonce_str", noncestr);
            parameters.Add("times_tamp", timestamp);
            parameters.Add("card_id", cardId);
            parameters.Add("card_type", cardType);
            return CreateCardSha1(parameters);
        }

        /// <summary>
        ///获取添加卡券时Ext参数内的签名 
        /// </summary>
        /// <param name="api_ticket"></param>
        /// <param name="timestamp"></param>
        /// <param name="card_id"></param>
        /// <param name="nonce_str"></param>
        /// <param name="code"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public static string GetcardExtSign(string api_ticket, string timestamp, string card_id, string nonce_str = "",
            string code = "", string openid = "")
        {
            var parameters = new Hashtable();
            parameters.Add("api_ticket", api_ticket);
            parameters.Add("timestamp", timestamp);
            parameters.Add("card_id", card_id);
            if (!string.IsNullOrEmpty(code))
            {
                parameters.Add("code", code);
            }
            if (!string.IsNullOrEmpty(openid))
            {
                parameters.Add("openid", openid);
            }
            if (!string.IsNullOrEmpty(nonce_str))
            {
                parameters.Add("nonce_str", nonce_str);
            }
            return CreateNonekeySha1(parameters);
        }


        
    }
}

