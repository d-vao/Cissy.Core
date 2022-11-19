using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Cissy.Caching;
using Cissy.Configuration;
using Cissy.WeiXin.Https;
using Cissy.Http;

namespace Cissy.WeiXin
{
    internal partial class DefaultWeiXinMpApi
    {
        /// <summary>
        /// 获取视频素材
        /// </summary>
        /// <param name="media_id">素材Id</param>
        /// <returns></returns>
        public async Task<MediaResult> GetVideoMedia(string media_id)
        {
            var urlFormat = WeiXinApiHelper.BaseCgiBin + "/media/get?access_token={0}" + $"&media_id={media_id}";
            return await CommonJsonSend.SendAsync<MediaResult, StringRef>(await this.GetAccessTokenAsync(), urlFormat, null, CommonJsonSendType.GET);
        }
        /// <summary>
        /// 获取图片素材
        /// </summary>
        /// <param name="media_id">素材Id</param>
        /// <returns></returns>
        public async Task<Stream> GetImageMedia(string media_id)
        {
            var url = WeiXinApiHelper.BaseCgiBinssl + $"/media/get?access_token={await this.GetAccessTokenAsync()}&media_id={media_id}";
            return await Actor.Public.DownloadAsync(url);
        }
    }
}
