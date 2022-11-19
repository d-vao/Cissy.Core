using System;
using System.Collections.Generic;
using System.Text;

namespace Cissy.PCApp
{
    /// <summary>
    /// 应用配置信息
    /// </summary>
    public class PCAppConfig : IModel
    {
        /// <summary>
        /// 用户配置接口地址
        /// </summary>
        public string UserConfigApiUri { get; set; }
        /// <summary>
        /// 最新版本
        /// </summary>
        public float LatestVersion { get; set; }
    }
}
