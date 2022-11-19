using System;
using System.Collections.Generic;
using System.Text;

namespace Cissy.WeiXin
{
    /// <summary>
    /// 二维码类型
    /// </summary>
    public enum QRCodeActionName
    {
        /// <summary>
        /// 临时的整型参数值
        /// </summary>
        QR_SCENE = 0,
        /// <summary>
        /// 临时的字符串参数值
        /// </summary>
        QR_STR_SCENE = 3,
        /// <summary>
        /// 永久的整型参数值
        /// </summary>
        QR_LIMIT_SCENE = 1,
        /// <summary>
        /// 永久的字符串参数值
        /// </summary>
        QR_LIMIT_STR_SCENE = 2
    }
}
