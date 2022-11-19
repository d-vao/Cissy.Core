using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cissy.Configuration;

namespace Cissy.WeiXin
{
    public interface IWeiXinMpAccessToken
    {
        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <returns></returns>
        Task<string> GetAccessTokenAsync();
        /// <summary>
        /// 刷新AccessToken
        /// </summary>
        /// <returns></returns>
        Task<string> FreshAccessToken();
    }
}
