using System;
using System.Collections.Generic;
using System.Text;
using Cissy.Caching;
using Cissy.Configuration;
using Cissy.WeiXin.Https;

namespace Cissy.WeiXin
{
    internal partial class DefaultWeiXinMpApi
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
        public async System.Threading.Tasks.Task<CreateQrCodeResult> CreateQrCodeAsync(int expireSeconds, int sceneId, QRCodeActionName actionName, string sceneStr = null, int timeOut = 10000)
        {
            var urlFormat = WeiXinApiHelper.BaseCgiBinssl + "/qrcode/create?access_token={0}";
            CreateQrCodeRequestBase data = default(CreateQrCodeRequestBase);

            switch (actionName)
            {
                case QRCodeActionName.QR_SCENE:
                    data = new QR_SCENE()
                    {
                        expire_seconds = expireSeconds,
                        action_name = "QR_SCENE",
                        action_info = new ActionInfo()
                        {
                            scene = new Scene_Id()
                            {
                                scene_id = sceneId
                            }
                        }
                    };
                    break;
                case QRCodeActionName.QR_LIMIT_SCENE:
                    data = new QR_LIMIT_SCENE()
                    {
                        action_name = "QR_LIMIT_SCENE",
                        action_info = new ActionInfo()
                        {
                            scene = new Scene_Id()
                            {
                                scene_id = sceneId
                            }
                        }
                    };
                    break;
                case QRCodeActionName.QR_LIMIT_STR_SCENE:
                    data = new QR_LIMIT_STR_SCENE()
                    {
                        action_name = "QR_LIMIT_STR_SCENE",
                        action_info = new ActionInfo()
                        {
                            scene = new Scene_Str()
                            {
                                scene_str = sceneStr
                            }
                        }
                    };
                    break;
                case QRCodeActionName.QR_STR_SCENE:
                    data = new QR_STR_SCENE()
                    {
                        expire_seconds = expireSeconds,
                        action_name = "QR_STR_SCENE",
                        action_info = new ActionInfo()
                        {
                            scene = new Scene_Str()
                            {
                                scene_str = sceneStr
                            }
                        }
                    };
                    break;
                default:
                    //throw new ArgumentOutOfRangeException(nameof(actionName), actionName, null);
                    throw new ArgumentOutOfRangeException(actionName.GetType().Name, actionName, null);
            }

            return await CommonJsonSend.SendAsync<CreateQrCodeResult, CreateQrCodeRequestBase>(await this.GetAccessTokenAsync(), urlFormat, data);

        }

    }
}
