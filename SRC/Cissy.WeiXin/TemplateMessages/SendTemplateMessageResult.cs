using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cissy.WeiXin.Https;

namespace Cissy.WeiXin.TemplateMessages
{
    /// <summary>
    /// 发送模板消息结果
    /// </summary>
    public class SendTemplateMessageResult : WxJsonResult
    {
        /// <summary>
        /// msgid
        /// </summary>
        public long msgid { get; set; }
    }
}
