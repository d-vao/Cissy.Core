using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cissy.Configuration;

namespace Cissy.WeiXin
{
    public interface IWeiXinMpQRCode
    {
        /// <summary>
        /// 创建二维码
        /// </summary>
        /// <param name="accessToken">AccessToken</param>
        /// <param name="expireSeconds">临时二维码有效时间，以秒为单位。最大不超过2592000（即30天），此字段如果不填，则默认有效期为30秒,永久二维码将忽略此参数</param>
        /// <param name="sceneId">场景值ID，临时二维码时为32位非0整型，永久二维码时最大值为100000（目前参数只支持1--100000）</param>
        /// <param name="sceneStr">场景字符串，字符串类型，长度限制为1到64，仅actionName为QR_LIMIT_STR_SCENE时有效</param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <param name="actionName">二维码类型，当actionName为QR_LIMIT_STR_SCENE时，sceneId会被忽略</param>
        /// <returns></returns>
        Task<CreateQrCodeResult> CreateQrCodeAsync(int expireSeconds, int sceneId, QRCodeActionName actionName, string sceneStr = null, int timeOut = 10000);
    }
}
