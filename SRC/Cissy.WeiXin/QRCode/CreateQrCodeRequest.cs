using System;
using System.Collections.Generic;
using System.Text;

namespace Cissy.WeiXin
{
    internal interface IScene
    {

    }
    internal class Scene_Id : IScene
    {
        public int scene_id;
    }
    internal class Scene_Str : IScene
    {
        public string scene_str;
    }
    internal class ActionInfo
    {
        public IScene scene;
    }
    internal abstract class CreateQrCodeRequestBase : IModel
    {
        public string action_name;
        public ActionInfo action_info;
    }
    internal class QR_SCENE : CreateQrCodeRequestBase
    {
        public int expire_seconds;
    }
    internal class QR_LIMIT_SCENE : CreateQrCodeRequestBase
    {

    }
    internal class QR_LIMIT_STR_SCENE : CreateQrCodeRequestBase
    {

    }
    internal class QR_STR_SCENE : CreateQrCodeRequestBase
    {
        public int expire_seconds;
    }
}
