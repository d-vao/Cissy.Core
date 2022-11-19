using System;
using System.Collections.Generic;
using System.Text;
using Cissy.Configuration;

namespace Cissy.WeiXin
{
    public interface IWeiXinMpApi : IWeiXinMpAccessToken, IWeiXinMpQRCode, IWeiXinMpOCR, ICommonApi, IWeiXinMpMedia, IWeiXinApp
    {
        AppConfig Config { get; }
    }
}
