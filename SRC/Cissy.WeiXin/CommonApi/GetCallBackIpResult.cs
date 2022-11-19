using Cissy.WeiXin.Https;
namespace Cissy.WeiXin
{
    /// <summary>
    /// 获取微信服务器的 IP 段后的 JSON 返回格式
    /// </summary>
    public class GetCallBackIpResult : WxJsonResult
    {
        public string[] ip_list { get; set; }
    }
}
