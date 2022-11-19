using Cissy.WeiXin.Https;

namespace Cissy.WeiXin
{
    /// <summary>
    /// 小程序登录凭证校验
    /// </summary>
    public class Code2SessionResult : WxJsonResult
    {
        public string openid { get; set; }
        public string session_key { get; set; }
        public string unionid { get; set; }
    }

}
