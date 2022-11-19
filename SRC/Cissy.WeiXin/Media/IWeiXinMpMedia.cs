using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Cissy.Configuration;

namespace Cissy.WeiXin
{
    public interface IWeiXinMpMedia
    {
        /// <summary>
        /// 获取视频素材
        /// </summary>
        /// <param name="media_id">素材Id</param>
        /// <returns></returns>
       Task< MediaResult> GetVideoMedia(string media_id);
        /// <summary>
        /// 获取图片素材
        /// </summary>
        /// <param name="media_id">素材Id</param>
        /// <returns></returns>
        Task<Stream> GetImageMedia(string media_id);

    }
}
