using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Cissy.WeiXin.Https;

namespace Cissy.WeiXin.TemplateMessages
{
    public interface INotice
    {
        string GetTemplateID();
    }
    public class NoticeModel<T> where T : INotice
    {
        public NoticeModel(T notice)
        {
            this.Data = notice;
        }
        public string OpenID { get; set; }
        public string TopColor { get; set; }
        public string Url { get; set; }
        public T Data { get; private set; }
        public async Task SendAsync(string AccessToken)
        {
            //try
            //{
            await TemplateMessageHelper.SendTemplateMessageAsync(NoticeExt.TemplateMessageUrlFormat, AccessToken, this.OpenID, this.Data.GetTemplateID(), this.TopColor, this.Url, this.Data);
            //}
            //catch(Exception e)
            //{
            //    //Thread.Sleep(100);
            //    //TemplateMessageHelper.SendTemplateMessage(MPHelper.TemplateMessageUrlFormat, Actor.Public.GetAccessToken(), this.OpenID, this.Data.GetTemplateID(), this.TopColor, this.Url, this.Data, 100);
            //}
        }


    }

    public static class NoticeExt
    {
        public const string TemplateSetIndustryUrlFormat = "https://api.weixin.qq.com/cgi-bin/template/api_set_industry?access_token={0}";
        public const string TemplateMessageUrlFormat = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";
        /// <summary>
        /// 发送通知
        /// </summary>
        /// <param name="Public"></param>
        /// <param name="Notice"></param>
        public static void AsyncSendNotices<T>(this Public Public, string AccessToken, NoticeModel<T>[] NoticeModels) where T : INotice
        {
            Task.Factory.StartNew(async () =>
                {
                    foreach (NoticeModel<T> m in NoticeModels)
                    {
                        await m.SendAsync(AccessToken);
                    }
                });
        }
        /// <summary>
        /// 发送通知
        /// </summary>
        /// <param name="Public"></param>
        /// <param name="Notice"></param>
        public static async Task SendNoticesAsync<T>(this Public Public, string AccessToken, NoticeModel<T>[] NoticeModels) where T : INotice
        {
            foreach (NoticeModel<T> m in NoticeModels)
            {
                await m.SendAsync(AccessToken);
            }
        }
        public static async Task<WxJsonResult> SetIndustryAsync(string accessToken, string industry_id1, string industry_id2, int timeOut = 300)
        {
            TemplateIndustry TemplateIndustry = new TemplateIndustry()
            {
                industry_id1 = industry_id1,
                industry_id2 = industry_id2
            };
            return await CommonJsonSend.SendAsync<TemplateIndustry>(accessToken, NoticeExt.TemplateSetIndustryUrlFormat, TemplateIndustry);
        }
        /// <summary>
        /// 查询是否关注了本公众号
        /// </summary>
        /// <param name="Public"></param>
        /// <param name="OpenID"></param>
        /// <returns></returns>
        //public static bool IsFocusMe(this Public Public, string OpenID)
        //{
        //    string url = string.Format(MPHelper.UserInfoUrlFormat, Public.GetAccessToken(), OpenID, "zh_CN ");
        //    string response = Actor.Public.HttpPost(url);//HtmlWeb.GetInstance().Post(url);
        //    UserBaseInfo UserBaseInfo = Actor.Public.Deserialize<UserBaseInfo>(response);
        //    if (UserBaseInfo.IsNotNull())
        //    {
        //        return UserBaseInfo.subscribe != "0";
        //    }
        //    return false;
        //}
    }
}
