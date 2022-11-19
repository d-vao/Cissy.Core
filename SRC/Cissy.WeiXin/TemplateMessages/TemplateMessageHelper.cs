using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cissy.WeiXin.Https;

namespace Cissy.WeiXin.TemplateMessages
{
    public static class TemplateMessageHelper
    {
        public static async System.Threading.Tasks.Task<SendTemplateMessageResult> SendTemplateMessageAsync(string urlFormat, string accessToken, string openId, string templateId, string topcolor, string url, object data)
        {
            var msgData = new TempleteModel()
            {
                touser = openId,
                template_id = templateId,
                topcolor = topcolor,
                url = url,
                data = data
            };
            return await CommonJsonSend.SendAsync<SendTemplateMessageResult, TempleteModel>(accessToken, urlFormat, msgData);
        }
    }
}
